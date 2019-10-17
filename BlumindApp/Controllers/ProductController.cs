using BlumindApp.Services;
using Commom.Models.Product;
using Entities.BlumindDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Validation.Creator;

namespace BlumindApp.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class ProductController : BaseController {
        private readonly ProductService _service;

        public ProductController(ProductService productService)
        {
            _service = productService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostProduct([FromBody] ProductPostModel model)
        {
            var validator = ValidatorFactory.GetValidator<ProductPostModel>();
            validator.Validate(model);
            await _service.SaveProduct(model, CurrentUserId);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProduct([FromQuery] int productId)
        {
            using (var db = new BlumindbaseContext())
            {
                var model = await db.Products.FirstOrDefaultAsync(p => p.Id == productId);

                return Ok(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProducts()
        {
            using (var db = new BlumindbaseContext())
            {
                var model = db.Products.Select(p => new { p.Id, p.Name }).ToList();

                return Ok(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _service.GetProducts();
            
            return Ok(products);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteProduct([FromQuery] int productId)
        {
            await _service.DeleteProduct(productId);
            return Ok();
        }

    }

}
