namespace BaseLibrary.Models.Products
{
    public class ProductReview
    {
        public int Id { get; set; }
        public string? Content { get; set; } // Nội dung
        public int? Rating { get; set; } // Đánh giá
        public DateOnly CreatedAt { get; set; } // Ngày tạo
        public DateOnly? UpdatedAt { get; set; } // Ngày cập nhật

        // Quan hệ đến bảng Sản phẩm: Nhiều - Một
        public int ProductId { get; set; } // Mã sản phẩm
        public Product? Product { get; set; } // Sản phẩm

        // Quan hệ đến bảng Khách hàng: Nhiều - Một
        public int CustomerId { get; set; } // Mã khách hàng
        public Customer? Customer { get; set; } // Khách hàng
    }
}
