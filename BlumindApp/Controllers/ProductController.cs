using BlumindApp.Models.Product;
using Entities.BlumindDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlumindApp.Controllers {
    [Route("api/[controller]")]
    public class ProductController : BaseController {
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> PostProduct([FromBody] ProductPostModel model)
        {
            using (var db = new BlumindbaseContext())
            {
                var entity = db.Products.FirstOrDefault(p => p.Id == model.Id);

                if (entity == null)
                {
                    entity = new Product
                    {
                        Name = model.Name,
                        ValidFrom = DateTime.Now,
                        Quantity = model.Quantity,
                        Price = model.Price,
                        UserInsert = CurrentUserId,
                        DateInsert = DateTime.Now
                    };
                    db.Products.Add(entity);
                }
                else
                {
                    entity.Name = model.Name;
                    entity.ValidFrom = model.ValidFrom;
                    entity.Quantity = model.Quantity;
                    entity.Price = model.Price;
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

    }

}
