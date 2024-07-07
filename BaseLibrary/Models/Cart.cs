using BaseLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BaseLibrary.DTOs.CartDTO;

namespace BaseLibrary.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();
        public class CartLine
        {
            public int CartId { get; set; }
            public ProductCartItem productItem { get; set; } = new();
            public int Quantity { get; set; }
        }
    }
}
