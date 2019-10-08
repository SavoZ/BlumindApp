using Commom.Models.Category;
using Commom.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;
using Validation.Interfaces;
using Validation.Validators;

namespace Validation.Creator {
    public class ValidatorFactory {
        public static IValidator<T> GetValidator<T>() where T : class
        {

            if (typeof(T).IsAssignableFrom(typeof(ProductPostModel)))
            {
                return (IValidator<T>)new ProductValidator();
            }

            if (typeof(T).IsAssignableFrom(typeof(CategoryPostModel)))
            {
                return (IValidator<T>)new CategoryValidator();
            }

  

            throw new ArgumentOutOfRangeException(nameof(T), null, null);
        }

    }
}
