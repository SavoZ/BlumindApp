using System;
using System.Collections.Generic;

namespace Entities.BlumindDB
{
    public partial class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public string UserInsert { get; set; }
        public DateTime DalteInsert { get; set; }
        public string UserModified { get; set; }
        public DateTime? DateModified { get; set; }
    }
}