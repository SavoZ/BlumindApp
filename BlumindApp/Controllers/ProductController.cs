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
    public class ProductController : Controller {
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> PostProduct([FromBody] ProductEditModel model)
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
                        Price = model.Price
                    };

                    db.Products.Add(entity);
                    await db.SaveChangesAsync();
                }
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

    }

}
