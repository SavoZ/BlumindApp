using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlumindApp.Models.Product {
    public class ProductPostModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
