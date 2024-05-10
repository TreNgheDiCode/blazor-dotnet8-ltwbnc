namespace BaseLibrary.Models.Products
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string? Url { get; set; } // Đường dẫn ảnh

        // Quan hệ đến bảng Sản phẩm: Nhiều - Một
        public int ProductId { get; set; }
        public Product? Product { get; set; } // Sản phẩm
    }
}
