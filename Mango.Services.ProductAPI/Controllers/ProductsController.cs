using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDTO _response;
        private readonly IMapper _mapper;
        public ProductsController(AppDbContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
            _response = new ResponseDTO();
        }

        [HttpGet]
        public ResponseDTO Get()
        {
            try
            {
                IEnumerable<Product> objList = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDTO>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("{id:int}")]
        public ResponseDTO Get(int id)
        {
            try
            {
                Product obj = _db.Products.First(c => c.ProductId == id);

                _response.Result = _mapper.Map<ProductDTO>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }

        [HttpGet("GetByName/{name}")]
        public ResponseDTO GetByCode(string name)
        {
            try
            {
                Product obj = _db.Products.First(c => c.Name.ToLower() == name.ToLower());
                if (obj is null)
                {
                    _response.IsSuccess = false;
                }
                _response.Result = _mapper.Map<ProductDTO>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRATOR")]

        public async Task<ResponseDTO> Post(ProductDTO ProductDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(ProductDTO);
                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();

                if (ProductDTO.Image != null)
                {
                    string filename = product.ProductId + Path.GetExtension(ProductDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + filename;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        await ProductDTO.Image.CopyToAsync(fileStream);
                    }
                    var baseUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value + HttpContext.Request.PathBase.Value;
                    product.ImageUrl += baseUrl + "/ProductImages/" + filename;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = "https://placehold.co/600x400";
                }
                _db.Products.Update(product);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<ResponseDTO> Put(ProductDTO ProductDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(ProductDTO);

                if (ProductDTO.Image != null)
                {

                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        //Delete old image before uplaoding another one
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    string filename = product.ProductId + Path.GetExtension(ProductDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + filename;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        await ProductDTO.Image.CopyToAsync(fileStream);
                    }
                    var baseUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value + HttpContext.Request.PathBase.Value;
                    product.ImageUrl += baseUrl + "/ProductImages/" + filename;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = "https://placehold.co/600x400";
                }
                _db.Products.Update(product);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMINISTRATOR")]

        public ResponseDTO Delete(int id)
        {
            try
            {
                Product obj = _db.Products.First(c => c.ProductId == id);

                if (!string.IsNullOrEmpty(obj.ImageLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), obj.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }

                _db.Products.Remove(obj);
                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }
    }
}
