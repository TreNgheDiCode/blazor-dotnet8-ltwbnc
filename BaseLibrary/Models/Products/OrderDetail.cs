using System.ComponentModel.DataAnnotations.Schema;

namespace BaseLibrary.Models.Products
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        // Quan hệ đến bảng Giảm giá: Một chi tiết đơn hàng chỉ có một giảm giá
        [ForeignKey("Discount")]
        public int? DiscountId { get; set; }
        public Discount? Discount { get; set; }
        // Quan hệ đến bảng Sản phẩm: Một chi tiết đơn hàng chỉ thuộc về một sản phẩm
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; } = null!;

        // Quan hệ đến bảng Đơn hàng: Một chi tiết đơn hàng chỉ thuộc về một đơn hàng
        public string OrderId { get; set; } = null!;
        public Order? Order { get; set; }

        // Quan hệ đến bảng Tùy chọn sản phẩm: Một chi tiết đơn hàng chỉ thuộc về một tùy chọn sản phẩm
        public int ProductOptionId { get; set; }
        public ProductOption? ProductOption { get; set; }
    }
}
