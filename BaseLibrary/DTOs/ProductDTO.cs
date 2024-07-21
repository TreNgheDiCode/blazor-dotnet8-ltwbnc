using BaseLibrary.Enums;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.DTOs
{
    public record ProductList
    {
        public int TotalCount { get; init; }
        public int TotalPages { get; init; }
        public int CurrentPage { get; init; }
        public int PageSize { get; init; }
        public List<ProductItem> Products { get; init; } = [];
    }

    public record ProductItem
    {
        public int Id { get; set; } // ID sản phẩm
        public string? CoverUrl { get; set; } // Đường dẫn ảnh bìa
        public string? Name { get; set; } // Tên sản phẩm
        public string? Description { get; set; } // Mô tả sản phẩm
        public double? Price { get; set; } // Giá gốc
        public int? Discount { get; set; } // % Giảm giá
        public bool IsFlashSale { get; set; } // Có đang giảm giá
        public ProductStatus Status { get; set; } // Trạng thái sản phẩm
        public string? CategoryName { get; set; } // Danh mục
        public List<ProductItemImage>? ProductImages { get; set; } // Danh sách hình ảnh sản phẩm
        public List<ProductItemOption>? ProductOptions { get; set; } // Danh sách tùy chọn sản phẩm
        public List<ProductItemReview>? ProductReviews { get; set; } // Danh sách đánh giá
    }

    public record ProductItemImage
    {
        public int Id { get; set; } // ID hình ảnh
        [Required(ErrorMessage = "Đường dẫn hình ảnh không được để trống")]
        public string? ImageUrl { get; set; } // Đường dẫn hình ảnh
    }

    public record ProductItemOption
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Màu sắc không được để trống")]
        public string? Color { get; set; } // Màu sắc
        [Required(ErrorMessage = "Kích thước không được để trống")]
        public string? Size { get; set; } // Kích thước
        [Required(ErrorMessage = "Số lượng không được để trống")]
        public int Quantity { get; set; } // Số lượng
    }

    public record ProductItemReview
    {
        public int Id { get; set; } // ID đánh giá
        public string? Content { get; set; } // Nội dung đánh giá
        public int? Rating { get; set; } //  Đánh giá
        public DateOnly CreatedAt { get; set; } // Ngày tạo
        public DateOnly? UpdatedAt { get; set; } // Ngày cập nhật
        public ProductItemReviewUserDetail? User { get; set; } // Thông tin người dùng
    }

    public record ProductItemReviewUserDetail
    {
        public int Id { get; set; } // ID người dùng
        public string? FullName { get; set; } // Họ tên
        public string? Photo { get; set; } // Đường dẫn ảnh đại diện
    }

    public record CreateProductDTO
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string? Name { get; set; } // Tên sản phẩm
        [Required(ErrorMessage = "Ảnh bìa không được để trống")]
        public string? CoverUrl { get; set; } // Đường dẫn ảnh bìa
        public string? Description { get; set; } // Mô tả sản phẩm
        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [DataType(DataType.Currency)]
        [Range(80000, double.MaxValue, ErrorMessage = "Giá sản phẩm phải ít nhất 80000")]
        public double Price { get; set; } // Giá gốc
        [Range(0, 100, ErrorMessage = "Giảm giá phải từ 0 đến 100 (%)")]
        public int? Discount { get; set; } // % Giảm giá
        public bool IsFlashSale { get; set; } // Có đang giảm giá
        public ProductStatus Status { get; set; } // Trạng thái sản phẩm
        [Required(ErrorMessage = "Danh mục không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Danh mục không hợp lệ")]
        public int CategoryId { get; set; } // ID danh mục
        public List<ProductImageDTO>? ProductImages { get; set; } // Danh sách hình ảnh sản phẩm
        [Required(ErrorMessage = "Tùy chọn sản phẩm không được để trống")]
        [MinLength(1, ErrorMessage = "Tùy chọn sản phẩm phải ít nhất 1")]
        public List<ProductOptionDTO>? ProductOptions { get; set; } // Danh sách tùy chọn sản phẩm
    }

    public record ProductImageDTO
    {
        public string? ImageUrl { get; set; } // Đường dẫn hình ảnh
    }

    public record ProductOptionDTO
    {
        [Required(ErrorMessage = "Màu sắc không được để trống")]
        public string? Color { get; set; } // Màu sắc
        [Required(ErrorMessage = "Kích thước không được để trống")]
        public string? Size { get; set; } // Kích thước
        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải ít nhất 1")]
        public int Quantity { get; set; } // Số lượng
    }

    public record UpdateProductDTO
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string? Name { get; set; } // Tên sản phẩm
        [Required(ErrorMessage = "Ảnh bìa không được để trống")]
        public string? CoverUrl { get; set; } // Đường dẫn ảnh bìa
        public string? Description { get; set; } // Mô tả sản phẩm
        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [DataType(DataType.Currency)]
        [Range(80000, double.MaxValue, ErrorMessage = "Giá sản phẩm phải ít nhất 80000")]
        public double Price { get; set; } // Giá gốc
        [Range(0, 100, ErrorMessage = "Giảm giá phải từ 0 đến 100 (%)")]
        public int? Discount { get; set; } // % Giảm giá
        public bool IsFlashSale { get; set; } // Có đang giảm giá
        public ProductStatus Status { get; set; } // Trạng thái sản phẩm
        [Required(ErrorMessage = "Danh mục không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Danh mục không hợp lệ")]
        public int CategoryId { get; set; } // ID danh mục
        public List<ProductImageDTO>? ProductImages { get; set; } // Danh sách hình ảnh sản phẩm
        [MinLength(1, ErrorMessage = "Tùy chọn sản phẩm phải ít nhất 1")]
        public List<ProductOptionDTO>? ProductOptions { get; set; } // Danh sách tùy chọn sản phẩm
    }
}
