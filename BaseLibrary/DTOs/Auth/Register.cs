using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.DTOs.Auth
{
    public class Register : AccountBase
    {
        // Mật khẩu
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Mật khẩu không trùng khớp")]
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        public string? ConfirmPassword { get; set; }
        // Tên đầy đủ
        [Required(ErrorMessage = "Vui lòng nhập tên đầy đủ")]
        [MinLength(5, ErrorMessage = "Độ dài ít nhất 5 ký tự")]
        [MaxLength(100, ErrorMessage = "Độ dài nhiều nhất 100 ký tự")]
        public string? Fullname { get; set; }
        // Số điện thoại
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^0[0-9]{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }
        // Địa chỉ
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mã phường/xã/thị trấn")]
        public string? WardId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mã quận/huyện")]
        public string? DistrictId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mã thành phố/tỉnh")]
        public string? ProvinceId { get; set; }
        // Ảnh đại diện
        public string? Photo { get; set; }
    }
}
