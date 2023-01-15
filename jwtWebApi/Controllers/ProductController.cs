using jwtWebApi.Data;
using jwtWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace jwtWebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController, Authorize]
	public class ProductController : ControllerBase
	{
		private readonly DbContextClass _context;
		public ProductController(DbContextClass context)
		{
			_context = context;
		}
		[HttpGet]
		[Route("ProductsList")]
		public async Task<ActionResult<IEnumerable<Product>>> Get()
		{
			var products = new List<Product>();
			string msg = string.Empty;
			try
			{
				var product = await _context.Products.ToListAsync();

				if (product.Count > 0)
				{
					products.AddRange(product);
				}

			}
			catch (Exception ex)
			{
				msg = ex.Message;
			}

			return products;
		}
		[HttpGet]
		[Route("ProductDetail")]
		public async Task<ActionResult<Product>> Get(int id)
		{
			Product product = new Product();
			string msg = string.Empty;
			try
			{
				var pro = await _context.Products.FindAsync(id);
				if (pro == null)
				{
					return NotFound();
				}
				product = pro;
			}
			catch (Exception ex)
			{
				msg = ex.Message;
			}
			return product;
		}
		[HttpPost]
		[Route("CreateProduct")]
		public async Task<ActionResult<Product>> POST(Product product)
		{
			string msg = string.Empty;
			try
			{
				_context.Products.Add(product);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				msg = ex.Message;
			}
			return CreatedAtAction(nameof(Get), new
			{
				id = product.ProductId
			}, product);
		}
		[HttpPost]
		[Route("DeleteProduct")]
		public async Task<ActionResult<IEnumerable<Product>>> Delete(int id)
		{
			string msg = string.Empty;
			try
			{
				var product = await _context.Products.FindAsync(id);
				if (product == null)
				{
					return NotFound();
				}
				_context.Products.Remove(product);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				msg = ex.Message;
			}
			return await _context.Products.ToListAsync();
		}
		[HttpPost]
		[Route("UpdateProduct")]
		public async Task<ActionResult<IEnumerable<Product>>> Update(int id, Product product)
		{
			string msg = string.Empty;
			try
			{
			if (id != product.ProductId)
			{
				return BadRequest();
			}
			var productData = await _context.Products.FindAsync(id);
			if (productData == null)
			{
				return NotFound();
			}
			productData.ProductCost = product.ProductCost;
			productData.ProductDescription = product.ProductDescription;
			productData.ProductName = product.ProductName;
			productData.ProductStock = product.ProductStock;
			await _context.SaveChangesAsync();

			}
			catch (Exception ex)
			{
				msg = ex.Message;
			}
			return await _context.Products.ToListAsync();
		}
	}
}
