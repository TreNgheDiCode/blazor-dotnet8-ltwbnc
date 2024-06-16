using BaseLibrary.Models.Products;

namespace BaseLibrary.Models
{
    public class DiscountWarehouse
    {
        public int Id { get; set; }
        public bool IsUsed { get; set; } = false; // Đã sử dụng
        public int UserId { get; set; } // Mã khách hàng
        public ApplicationUser? User { get; set; } // Khách hàng
        public int DiscountId { get; set; } // Mã giảm giá
        public Discount? Discount { get; set; } // Giảm giá
    }
}
