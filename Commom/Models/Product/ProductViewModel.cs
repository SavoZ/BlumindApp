using System;
using System.Collections.Generic;
using System.Text;

namespace Commom.Models.Product {
    public class ProductViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
