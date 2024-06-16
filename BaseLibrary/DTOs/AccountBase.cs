using System.ComponentModel.DataAnnotations;


namespace BaseLibrary.DTOs
{
    public class AccountBase
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Mật khẩu phải chứa ít nhất 1 ký tự thường, 1 ký tự in hoa, 1 ký tự đặc biệt, 1 ký tự số.")]
        public string? Password { get; set; }
    }
}
