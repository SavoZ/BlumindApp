using System;
using System.Collections.Generic;
using System.Text;

namespace Validation.Interfaces {
    public interface IValidator<T> where T : class {
        void Validate(T model);
    }
}
