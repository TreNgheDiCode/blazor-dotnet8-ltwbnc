using BaseLibrary.Models.Products;
using System.Text.Json.Serialization;

namespace BaseLibrary.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; } // Số điện thoại
        public int AddressId { get; set; }
        public Address? Address { get; set; } // Địa chỉ
        public string? Photo { get; set; } // Ảnh đại diện
        public string? Other { get; set; } // Thông tin khác
        public DateOnly? CreatedAt { get; set; } // Ngày tạo
        public bool? IsLocked { get; set; } // Đã khóa

        // Quan hệ đến bảng Đánh giá: Một - Nhiều
        [JsonIgnore]
        public List<ProductReview>? ProductReviews { get; set; } // Danh sách đánh giá

        // Quan hệ đến bảng Giảm giá: Một - Nhiều
        [JsonIgnore]
        public List<Discount>? Discounts { get; set; } // Danh sách giảm giá

        // Quan hệ đến bảng Đơn hàng: Một - Nhiều
        [JsonIgnore]
        public List<Order>? Orders { get; set; } // Danh sách đơn hàng
    }
}
