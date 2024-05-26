using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private ResponseDTO _response;
        public ShoppingCartAPIController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDTO();

        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO> CartUpsert(CartDTO cartDTO)
        {
            try
            {
                var cartHeaderFromDb = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == cartDTO.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create cart header and cart detail
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDTO.CartHeader);
                    _context.CartHeaders.Add(cartHeader);
                    _context.SaveChangesAsync();
                    cartDTO.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _context.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                    await _context.SaveChangesAsync();

                }
                else
                {
                    //if header is not null
                    //check if details has the same product
                    var cartDetailsFromDb = await _context.CartDetails.FirstOrDefaultAsync(c => c.ProductId == cartDTO.CartDetails.First().ProductId
                    && c.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        //Created cartdetails
                    }
                    else
                    {
                        //update count in cart details
                    }

                }

            }
            catch (Exception e)
            {
                _response.Message = e.Message;
                _response.IsSuccess = false;
            }
        }
    }
}
