namespace BaseLibrary.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string? Street { get; set; } // Tên đường
        public string? Ward { get; set; } // Phường
        public string? District { get; set; } // Quận
        public string? City { get; set; } // Thành phố
        public string? ZipCode { get; set; } // Mã bưu chính
    }
}
