using Commom.Models.Category;
using Entities.BlumindDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Validation.Creator;

namespace BlumindApp.Controllers {
    [Route("api/[controller]")]
    public class CategoryController : BaseController {

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            using (var db = new BlumindbaseContext())
            {
                var model = await db.Categories.Select(p => new { p.Id, p.Name }).ToListAsync();

                return Ok(model);
            }
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> PostCategory([FromBody] CategoryPostModel model)
        {
            var validator = ValidatorFactory.GetValidator<CategoryPostModel>();
            validator.Validate(model);

            using (var db = new BlumindbaseContext())
            {
                var entity = db.Categories.FirstOrDefault(p => p.Id == model.Id);

                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
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
                            entity.ValidFrom = model.ValidFrom.Value;
                            entity.UserModified = CurrentUserId;
                            entity.DateModified = DateTime.Now;
                        }
                        await db.SaveChangesAsync();
                        model.Id = entity.Id;

                        await AddCategoryProducts(model);

                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return Ok();
        }

        private async Task AddCategoryProducts(CategoryPostModel model)
        {
            using (var db = new BlumindbaseContext())
            {
                var currentProducts = await db.CategoryProducts.Where(c => c.CategoryId == model.Id).ToListAsync();

                db.RemoveRange(currentProducts);

                foreach (var productId in model.Products)
                {
                    var entity = new CategoryProduct { CategoryId = model.Id.Value, ProductId = productId };
                    db.CategoryProducts.Add(entity);
                }

                await db.SaveChangesAsync();
            }
        }
    }
}
