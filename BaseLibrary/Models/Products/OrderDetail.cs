namespace BaseLibrary.Models.Products
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }

        // Quan hệ đến bảng Sản phẩm: Một chi tiết đơn hàng chỉ thuộc về một sản phẩm
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        // Quan hệ đến bảng Đơn hàng: Một chi tiết đơn hàng chỉ thuộc về một đơn hàng
        public string OrderId { get; set; } = null!;
        public Order? Order { get; set; }

        // Quan hệ đến bảng Tùy chọn sản phẩm: Một chi tiết đơn hàng chỉ thuộc về một tùy chọn sản phẩm
        public int ProductOptionId { get; set; }
        public ProductOption? ProductOption { get; set; }
    }
}
