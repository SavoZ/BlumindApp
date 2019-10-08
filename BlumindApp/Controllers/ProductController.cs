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
    [Route("api/[controller]")]
    public class ProductController : BaseController {
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> PostProduct([FromBody] ProductPostModel model)
        {
            var validator = ValidatorFactory.GetValidator<ProductPostModel>();
            validator.Validate(model);

            using (var db = new BlumindbaseContext())
            {
                var entity = db.Products.FirstOrDefault(p => p.Id == model.Id);

                if (entity == null)
                {
                    entity = new Product
                    {
                        Name = model.Name,
                        ValidFrom = DateTime.Now,
                        Quantity = model.Quantity.Value,
                        Price = model.Price.Value,
                        UserInsert = CurrentUserId,
                        DateInsert = DateTime.Now
                    };
                    db.Products.Add(entity);
                }
                else
                {
                    entity.Name = model.Name;
                    entity.ValidFrom = model.ValidFrom.Value;
                    entity.Quantity = model.Quantity.Value;
                    entity.Price = model.Price.Value;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = CurrentUserId;
                }
                await db.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> GetProduct()
        {
            using (var db = new BlumindbaseContext())
            {
                var model = db.Products.FirstOrDefault();

                return Ok(model);
            }
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllProducts()
        {
            using (var db = new BlumindbaseContext())
            {

                var model = db.Products.Select(p => new { p.Id, p.Name }).ToList();

                return Ok(model);
            }
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetProducts()
        {
            using (var db = new BlumindbaseContext())
            {
                var model = await (from p in db.Products
                                   join cp in db.CategoryProducts on p.Id equals cp.ProductId
                                   join c in db.Categories on cp.CategoryId equals c.Id
                                   select new ProductViewModel
                                   {
                                       Id = p.Id,
                                       Name = p.Name,
                                       Category = c.Name,
                                       Quantity = p.Quantity,
                                       Price = p.Price,
                                       ValidFrom = p.ValidFrom
                                   }).ToListAsync();

                return Ok(model);
            }
        }

    }

}
