namespace BaseLibrary.DTOs.Auth
{
    public class UserSession
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
