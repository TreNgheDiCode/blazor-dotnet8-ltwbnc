namespace BaseLibrary.Models.Products
{
    public class ProductOption
    {
        public int Id { get; set; }
        public string? Color { get; set; } // Màu sắc
        public string? Size { get; set; } // Kích thước
        public int Quantity { get; set; } // Số lượng

        // Quan hệ đến bảng Sản phẩm: Nhiều - Một
        public int ProductId { get; set; }
        public Product? Product { get; set; } // Sản phẩm
    }
}
