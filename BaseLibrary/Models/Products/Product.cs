using BaseLibrary.Enums;
using System.Text.Json.Serialization;

namespace BaseLibrary.Models.Products
{
    public class Product : BaseModel
    {
        public string? Description { get; set; } // Mô tả sản phẩm
        public double? Price { get; set; } // Giá gốc
        public int? Discount { get; set; } // % Giảm giá
        public bool? IsFlashSale { get; set; } // Có đang giảm giá
        public ProductStatus Status { get; set; } // Trạng thái sản phẩm

        // Quan hệ đến bảng Hình ảnh sản phẩm: Một - Nhiều
        [JsonIgnore]
        public List<ProductImage>? ProductImages { get; set; } // Danh sách hình ảnh sản phẩm

        // Quan hệ đến bảng Tùy chọn sản phẩm: Một - Nhiều
        [JsonIgnore]
        public List<ProductOption>? ProductOptions { get; set; } // Danh sách tùy chọn sản phẩm

        // Quan hệ đến bảng Đánh giá: Một - Nhiều
        [JsonIgnore]
        public List<ProductReview>? ProductReviews { get; set; } // Danh sách đánh giá

        // Quan hệ đến bảng Giảm giá sản phẩm: Một - Nhiều
        [JsonIgnore]
        public List<Discount>? ProductDiscounts { get; set; } // Danh sách giảm giá

        // Quan hệ đến bảng Danh mục: Nhiều - Một
        public int CategoryId { get; set; }
        public Category? Category { get; set; } // Danh mục sản phẩm
    }
}
