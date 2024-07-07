using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.DTOs
{
    public record DiscountList
    {
        public int TotalCount { get; init; }
        public int TotalPages { get; init; }
        public int CurrentPage { get; init; }
        public int PageSize { get; init; }
        public List<DiscountItem> Discounts { get; init; } = [];
    }

    public record DiscountItem
    {
        public int Id { get; set; } // ID giảm giá
        public string? Name { get; set; } // Tên giảm giá
        public int Value { get; set; } // % Giảm giá
        public int Quantity { get; set; } // Số lượng (Dùng trong trường hợp săn sale)
        public DateTime StartDate { get; set; } // Ngày bắt đầu
        public DateTime EndDate { get; set; } // Ngày kết thúc
        public DiscountProductDTO? Product { get; set; } // Sản phẩm
        public List<DiscountWarehouseItemDTO> Owners { get; set; } = [];
    }

    public record DiscountProductDTO
    {
        public int ProductId { get; set; } // Mã sản phẩm
        public string? Name { get; set; } // Tên sản phẩm
    }

    public record DiscountWarehouseItemDTO
    {
        public int UserId { get; set; } // Mã khách hàng
        public string? Name { get; set; } // Tên khách hàng
        public bool IsUsed { get; set; } // Đã sử dụng
    }

    public record CreateDiscountDTO
    {
        public string? Name { get; set; } // Tên giảm giá
        [Required(ErrorMessage = "Giá trị giảm giá không được để trống")]
        [Range(1, 80, ErrorMessage = "Mã giảm giá không hợp lệ")]
        public int Value { get; set; } // % Giảm giá
        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng không hợp lệ")]
        public int Quantity { get; set; } // Số lượng (Dùng trong trường hợp săn sale)
        public DateTime StartDate { get; set; } = DateTime.Now; // Ngày bắt đầu
        public DateTime EndDate { get; set; } = DateTime.Now; // Ngày kết thúc
        public int? ProductId { get; set; } // Mã sản phẩm
    }

    public record UpdateDiscountDTO
    {
        public string? Name { get; set; } // Tên giảm giá
        public int Value { get; set; } // % Giảm giá
        public int Quantity { get; set; } // Số lượng (Dùng trong trường hợp săn sale)
        public DateTime StartDate { get; set; } // Ngày bắt đầu
        public DateTime EndDate { get; set; } // Ngày kết thúc
        public int? ProductId { get; set; } // Mã sản phẩm
    }
}
