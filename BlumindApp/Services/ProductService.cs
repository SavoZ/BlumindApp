using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commom.Models.Product;
using Entities.BlumindDB;
using Microsoft.EntityFrameworkCore;

namespace BlumindApp.Services {
    public class ProductService {
        internal async Task SaveProduct(ProductPostModel model, string userId)
        {
            using (var db = new BlumindbaseContext())
            {
                var entity = db.Products.FirstOrDefault(p => p.Id == model.Id);

                if (entity == null)
                {
                    entity = new Product
                    {
                        UserInsert = userId,
                        DateInsert = DateTime.Now
                    };
                    db.Products.Add(entity);
                }
                else
                {
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = userId;
                }

                entity.Name = model.Name;
                entity.ValidFrom = model.ValidFrom.Value;
                entity.Quantity = model.Quantity.Value;
                entity.Price = model.Price.Value;
                await db.SaveChangesAsync();
            }
        }

        internal async Task<IEnumerable<ProductViewModel>> GetProducts()
        {
            using (var db = new BlumindbaseContext())
            {
                var products = await (from p in db.Products
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

                return products;
            }
        }

        internal async Task DeleteProduct(int productId)
        {
            using (var db = new BlumindbaseContext())
            {
                var product = await db.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product != null)
                {
                    var categoryProducts = await db.CategoryProducts.Where(c => c.ProductId == productId).ToListAsync();

                    db.CategoryProducts.RemoveRange(categoryProducts);
                    db.Products.Remove(product);

                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
