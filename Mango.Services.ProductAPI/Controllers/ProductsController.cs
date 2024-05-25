﻿using AutoMapper;
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

		public ResponseDTO Post([FromBody] ProductDTO ProductDTO)
		{
			try
			{
				Product obj = _mapper.Map<Product>(ProductDTO);
				_db.Products.Add(obj);
				_db.SaveChanges();

				_response.Result = _mapper.Map<ProductDTO>(obj);
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
		public ResponseDTO Put([FromBody] ProductDTO ProductDTO)
		{
			try
			{
				Product obj = _mapper.Map<Product>(ProductDTO);
				_db.Products.Update(obj);
				_db.SaveChanges();

				_response.Result = _mapper.Map<ProductDTO>(obj);
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