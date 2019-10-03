using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlumindApp.Models.Category {
    public class CategoryPostModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public List<int> Products { get; set; }

        public CategoryPostModel()
        {
            Products = new List<int>();
        }
    }
}
