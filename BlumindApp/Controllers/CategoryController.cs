using BlumindApp.Models.Category;
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

    public class CategoryController : BaseController {

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> PostCategory([FromBody] CategoryPostModel model)
        {
            using (var db = new BlumindbaseContext())
            {
                var entity = db.Categories.FirstOrDefault(p => p.Id == model.Id);
                if (entity == null)
                {
                    entity = new Category
                    {
                        Name = model.Name,
                        ValidFrom = DateTime.Now,
                        UserInsert = CurrentUserId,
                        DalteInsert = DateTime.Now
                    };
                    db.Categories.Add(entity);
                }
                else
                {
                    entity.Name = model.Name;
                    entity.ValidFrom = model.ValidFrom;
                    entity.UserModified = CurrentUserId;
                    entity.DateModified = DateTime.Now;
                }

                await db.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
