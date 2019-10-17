using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commom.Models.Category;
using Entities.BlumindDB;
using Microsoft.EntityFrameworkCore;

namespace BlumindApp.Services {
    public class CategoryService {
        internal async Task SaveCategory(CategoryPostModel model, string userId)
        {
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
                                UserInsert = userId,
                                DalteInsert = DateTime.Now
                            };
                            db.Categories.Add(entity);
                        }
                        else
                        {
                            entity.UserModified = userId;
                            entity.DateModified = DateTime.Now;
                        }

                        entity.Name = model.Name;
                        entity.ValidFrom = model.ValidFrom.Value;
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
