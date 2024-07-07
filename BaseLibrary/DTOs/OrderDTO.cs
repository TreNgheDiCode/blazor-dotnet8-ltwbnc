using BaseLibrary.Enums;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.DTOs
{
    public record OrderList
    {
        public int TotalCount { get; init; }
        public int TotalPages { get; init; }
        public int CurrentPage { get; init; }
        public int PageSize { get; init; }
        public List<OrderItem> Orders { get; init; } = [];
    }

    public record OrderItem
    {
        public string Id { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; } // Phương thức thanh toán
        public OrderStatus Status { get; set; } // Trạng thái đơn hàng
        public DateTime OrderDate { get; set; } // Ngày đặt hàng

        public OrderUser? User { get; set; }
        public List<OrderItemDetail>? OrderItemDetails { get; set; }
    }

    public record OrderUser
    {
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }

    public record OrderItemDetail
    {
        public int Id { get; set; }
        public string? CoverUrl { get; set; } // Ảnh bìa
        public string? Name { get; set; } // Tên sản phẩm
        public string? Size { get; set; } // Kích cỡ
        public string? Color { get; set; } // Màu sắc
        public double? Price { get; set; } // Giá gốc
        public int Quantity { get; set; } // Số lượng
        public int? FlashSale { get; set; } // Giá khuyến mãi
        public double Discount { get; set; } // Giảm giá
    }

    public record CreateOrderDTO
    {
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [MinLength(1, ErrorMessage = "Vui lòng chọn ít nhất 1 sản phẩm")]
        public List<CreateOrderItemDetail>? OrderItemDetails { get; set; }
    }

    public record CreateOrderItemDetail
    {
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
        public int? DiscountId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn tùy chọn sản phẩm")]
        public int ProductOptionId { get; set; }
    }

    public record UpdateOrderDTO
    {
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public List<UpdateOrderItemDetail>? OrderItemDetails { get; set; }
    }

    public record UpdateOrderItemDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public int ProductId { get; set; }
        public int ProductOptionId { get; set; }
    }
}
