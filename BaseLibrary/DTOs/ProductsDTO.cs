using BaseLibrary.Enums;
using BaseLibrary.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class ProductsDTO
    {

        public int Id { get; set; }  // For update operations, this will be used to identify the product
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public int? Discount { get; set; }
        public bool? IsFlashSale { get; set; }
        public ProductStatus Status { get; set; }
        public int CategoryId { get; set; }

    }
}
