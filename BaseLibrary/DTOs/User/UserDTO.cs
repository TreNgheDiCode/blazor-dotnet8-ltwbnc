namespace BaseLibrary.DTOs.User
{
    public record UserList
    {
        public int TotalCount { get; init; }
        public int TotalPages { get; init; }
        public int CurrentPage { get; init; }
        public int PageSize { get; init; }
        public List<UserItem> Users { get; init; } = [];
    }

    public record UserItem
    {
        public int Id { get; set; }
        public string? Fullname { get; set; } // Tên đầy đủ
        public string? Email { get; set; } // Email
        public string? PhoneNumber { get; set; } // Số điện thoại
        public string? Address { get; set; } // Địa chỉ
        public string? Photo { get; set; } // Ảnh đại diện
        public string? Other { get; set; } // Thông tin khác
        public DateOnly? CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now); // Ngày tạo
        public bool? IsLocked { get; set; } // Đã khóa
        public string? Role { get; set; } // Vai trò
    }

    public class UpdateUserDTO
    {
        public string? Fullname { get; set; } // Tên đầy đủ
        public string? Email { get; set; } // Email
        public string? PhoneNumber { get; set; } // Số điện thoại
        public AddressDetail? Address { get; set; } // Địa chỉ
        public string? Photo { get; set; } // Ảnh đại diện
        public string? Other { get; set; } // Thông tin khác
        public bool? IsLocked { get; set; } // Đã khóa
        public string? Role { get; set; } // Vai trò
    }

    public record AddressDetail
    {
        public string Address { get; set; } = string.Empty; // Đường
        public string WardId { get; set; } = string.Empty; // Phường
        public string DistrictId { get; set; } = string.Empty; // Quận
        public string ProvinceId { get; set; } = string.Empty; // Thành phố
    }
}
