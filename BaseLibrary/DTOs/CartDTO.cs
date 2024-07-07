using BaseLibrary.Enums;

namespace BaseLibrary.DTOs
{
    public class CartDTO
    {
        public record ProductCartItem
        {
            public int Id { get; set; } // ID sản phẩm
            public string? Name { get; set; } // Tên sản phẩm
            public string? Description { get; set; } // Mô tả sản phẩm
            public double? Price { get; set; } // Giá gốc
            public int? Discount { get; set; } // % Giảm giá
            public bool? IsFlashSale { get; set; } // Có đang giảm giá
            public ProductStatus Status { get; set; } // Trạng thái sản phẩm
            public CategoryItem? Category { get; set; } // Danh mục
            public int ProductItemOptions { get; set; } // Danh sách sản phâm
            public string? img { get; set; } // Danh sách hình ảnh sản phẩm
            public string? color { get; set; } // Danh sách tùy chọn sản phẩm
            public string? size { get; set; } // Danh sách tùy chọn sản phẩm
            
        }
    }
}
