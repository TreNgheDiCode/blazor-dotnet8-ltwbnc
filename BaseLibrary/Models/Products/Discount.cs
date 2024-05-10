namespace BaseLibrary.Models.Products
{
    public class Discount : BaseModel
    {
        public int Value { get; set; } // % Giảm giá
        public int Quantity { get; set; } // Số lượng (Dùng trong trường hợp săn sale)
        public DateTime StartDate { get; set; } // Ngày bắt đầu
        public DateTime EndDate { get; set; } // Ngày kết thúc

        // Mã giảm giá có thể không thuộc sở hữu của bất kì ai và có thể không thuộc về sản phẩm nào
        // Quan hệ đến bảng Sản phẩm: Nhiều - Một
        public int? ProductId { get; set; } // Mã sản phẩm
        public Product? Product { get; set; } // Sản phẩm

        // Quan hệ đến bảng Khách hàng: Nhiều - Một
        public int? CustomerId { get; set; } // Mã khách hàng
        public Customer? Customer { get; set; } // Khách hàng
    }
}
