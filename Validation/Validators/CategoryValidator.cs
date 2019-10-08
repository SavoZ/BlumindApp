using Commom.Models.Category;
using Entities.BlumindDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Validation.Interfaces;

namespace Validation.Validators {
    public class CategoryValidator : IValidator<CategoryPostModel> {
        public void Validate(CategoryPostModel model)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(model.Name))
            {
                errors.Add("Category name is required.");
            }
            else
            {
                using (var db = new BlumindbaseContext())
                {
                    var entity = db.Categories.Where(s => s.Name == model.Name).ToList();
                    if (entity.Any() && !model.Id.HasValue) errors.Add("Category with that name is already added.");
                }
            }
            if (!model.ValidFrom.HasValue)
            {
                errors.Add("ValidFrom  is required.");
            }

            var error = string.Join(';', errors);

            if (errors.Any()) throw new Exception(error);
        }
    }
}
