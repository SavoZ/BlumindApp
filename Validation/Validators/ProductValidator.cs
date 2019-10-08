using Commom.Models.Product;
using Entities.BlumindDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validation.Interfaces;

namespace Validation.Validators {
    public class ProductValidator : IValidator<ProductPostModel> {
        public void Validate(ProductPostModel model)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(model.Name))
            {
                errors.Add("Product name is required.");
            }
            else
            {
                using (var db = new BlumindbaseContext())
                {
                    var entity = db.Products.Where(s => s.Name == model.Name).ToList();
                    if (entity.Any() && !model.Id.HasValue) errors.Add("Product with that name is already added.");
                }
            }
            if (!model.ValidFrom.HasValue)
            {
                errors.Add("ValidFrom  is required.");
            }
            if (!model.Quantity.HasValue)
            {
                errors.Add("QuantityProduct name is required.");
            }
            if (!model.Price.HasValue)
            {
                errors.Add("Price is required.");
            }
            var error = string.Join(';', errors);

            if (errors.Any()) throw new Exception(error);
        }
    }
}
