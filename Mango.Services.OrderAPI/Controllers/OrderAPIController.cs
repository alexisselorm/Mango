using AutoMapper;
using Mango.MessageBus;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.DTO;
using Mango.Services.OrderAPI.Service.IService;
using Mango.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IProductService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private ResponseDTO _response;
        public OrderAPIController(AppDbContext db,
            IProductService productService,
            IMapper mapper,
            IConfiguration config,
            IMessageBus messageBus
            )
        {
            _db = db;
            _productService = productService;
            _mapper = mapper;
            _response = new ResponseDTO();
            _config = config;
            _messageBus = messageBus;

        }


        [Authorize]
        [HttpGet("GetOrders")]
        public async Task<ResponseDTO> Get(string userId = "")
        {
            try
            {
                IEnumerable<OrderHeader> orderList;
                if (User.IsInRole(StaticDetails.RoleAdmin))
                {
                    orderList = await _db.OrderHeaders.Include(o => o.OrderDetails).OrderByDescending(o => o.OrderHeaderId).ToListAsync();
                }
                else
                {
                    orderList = await _db.OrderHeaders.Include(o => o.OrderDetails).Where(u => userId == userId).OrderByDescending(o => o.OrderHeaderId).ToListAsync();

                }
                _response.Result = _mapper.Map<IEnumerable<OrderHeaderDTO>>(orderList);

            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }

        [Authorize]
        [HttpGet("GetOrder/{id:int")]
        public async Task<ResponseDTO> Get(int id)
        {
            try
            {
                OrderHeader orderHeader = await _db.OrderHeaders.Include(o => o.OrderDetails).FirstAsync(order => order.OrderHeaderId == id);
                _response.Result = _mapper.Map<OrderHeaderDTO>(orderHeader);
            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }


        [HttpPost("CreateOrder")]
        public async Task<ResponseDTO> CreateOrder([FromBody] CartDTO model)
        {
            try
            {
                OrderHeaderDTO orderHeaderDTO = _mapper.Map<OrderHeaderDTO>(model.CartHeader);
                orderHeaderDTO.OrderTime = DateTime.Now;
                orderHeaderDTO.Status = StaticDetails.Status_Pending;
                orderHeaderDTO.OrderDetails = _mapper.Map<IEnumerable<OrderDetailDTO>>(model.CartDetails);

                OrderHeader order = _db.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDTO)).Entity;
                await _db.SaveChangesAsync();

                orderHeaderDTO.OrderHeaderId = order.OrderHeaderId;
                _response.Result = orderHeaderDTO;

            }
            catch (Exception ex)
            {

                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }

        [HttpPost("CreateStripeSession")]
        [Authorize]
        public async Task<ResponseDTO> CreateSripeSession(StripeRequestDTO dto)
        {
            try
            {

                var options = new SessionCreateOptions
                {
                    SuccessUrl = dto.ApproveUrl,
                    CancelUrl = dto.CancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                if (dto.OrderHeader.Discount > 0)
                {
                    var DiscountjsObj = new List<SessionDiscountOptions>()
                    {
                        new SessionDiscountOptions
                        {
                            Coupon=dto.OrderHeader.CouponCode,
                        }
                    };
                    options.Discounts = DiscountjsObj;
                }


                foreach (var item in dto.OrderHeader.OrderDetails)
                {
                    var sessionlineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }
                        },
                        Quantity = item.Count,

                    };
                    options.LineItems.Add(sessionlineItem);
                }

                var service = new Stripe.Checkout.SessionService();
                Session session = service.Create(options);
                dto.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _db.OrderHeaders.First(u => u.OrderHeaderId == dto.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                await _db.SaveChangesAsync();
                _response.Result = dto;
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost("ValidateStripeSession")]
        [Authorize]
        public async Task<ResponseDTO> ValidateSripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                var orderHeader = _db.OrderHeaders.First(u => u.OrderHeaderId == orderHeaderId);


                var service = new Stripe.Checkout.SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);

                var paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    //Payment was successful
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = StaticDetails.Status_Approved;
                    await _db.SaveChangesAsync();

                    RewardDTO rewardDTO = new RewardDTO
                    {
                        OrderHeaderId = orderHeader.OrderHeaderId,
                        UserId = orderHeader.UserId,
                        RewardsActivity = (int)orderHeader.TotalAmount,
                    };
                    var topicName = _config["TopicAndQueueNames:OrderCreatedTopic"];

                    await _messageBus.PublishMessage(rewardDTO, topicName);

                    _response.Result = _mapper.Map<OrderHeaderDTO>(orderHeader);

                }

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        [Authorize]
        public async Task<ResponseDTO> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            try
            {
                OrderHeader orderHeader = await _db.OrderHeaders.FirstAsync(order => order.OrderHeaderId == orderId);
                if (orderHeader != null)
                {
                    if (newStatus == StaticDetails.Status_Cancelled)
                    {
                        //if order is cancelled, give a refund via stripe
                        var options = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = orderHeader.PaymentIntentId
                        };

                        var service = new RefundService();
                        Refund refund = service.Create(options);

                    }
                    orderHeader.Status = newStatus;
                    _db.OrderHeaders.Update(orderHeader);
                    await _db.SaveChangesAsync();


                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }
    }
}
