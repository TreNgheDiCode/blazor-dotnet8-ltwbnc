using BaseLibrary.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class ServiceModel<T>
    {
        public List<T>? List{ get; set; } = null;
        public T? Single { get; set; } 
        public String? Message { get; set; } =null;
        public bool? Success { get; set; } = true;
       
    }
}
