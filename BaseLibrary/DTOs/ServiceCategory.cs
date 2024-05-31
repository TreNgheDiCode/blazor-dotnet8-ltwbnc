using BaseLibrary.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class ServiceCategory
    {
        public List<Category>? ListCategory { get; set; } = null;
        public Category? SingleCategory { get; set; }
        public String? Message { get; set; } =null;
        public bool? Success { get; set; } = true;
       
    }
}
