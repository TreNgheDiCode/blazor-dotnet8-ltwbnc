using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.DTOs
{
    public class Register : AccountBase
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đầy đủ")]
        [MinLength(5, ErrorMessage = "Độ dài ít nhất 5 ký tự")]
        [MaxLength(100, ErrorMessage = "Độ dài nhiều nhất 100 ký tự")]
        public string? Fullname { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Mật khẩu không trùng khớp")]
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        public string? ConfirmPassword { get; set; }
    }
}
