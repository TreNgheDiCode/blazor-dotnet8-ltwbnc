using BaseLibrary.DTOs.Auth;
using BaseLibrary.Helpers.Client;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BaseLibrary.Helpers
{
    public class CustomAuthenticationStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        // Khởi tạo một người dùng ẩn danh (Không có phiên đăng nhập)
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());

        // Lấy thông tin trạng thái xác thực hiện tại
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Lấy token từ dịch vụ lưu trữ cục bộ
            var stringToken = await localStorageService.GetToken();

            // Nếu token rỗng hoặc null, trả về người dùng ẩn danh
            if (string.IsNullOrEmpty(stringToken))
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }

            // Giải mã token
            var deserializeToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
            if (deserializeToken == null)
            {
                // Nếu không thể giải mã token, trả về người dùng ẩn danh
                return await Task.FromResult(new AuthenticationState(anonymous));
            }

            // Lấy thông tin người dùng từ token
            var getUserClaims = DecryptToken(deserializeToken.Token!);
            if (getUserClaims == null)
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }

            // Tạo người dùng từ thông tin người dùng
            var claimsPrincipal = SetClaimPrincipal(getUserClaims);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        // Cập nhật trạng thái xác thực
        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            // Khởi tạo một người dùng ẩn danh
            var claimsPrincipal = new ClaimsPrincipal();

            // Nếu token hoặc refresh token không rỗng
            if (userSession.Token != null || userSession.RefreshToken != null)
            {
                // Chuyển phiên người dùng (jwt token + refresh token) thành chuỗi
                var serializeSession = Serializations.SerializeObj(userSession);

                // Lưu token vào dịch vụ lưu trữ cục bộ
                await localStorageService.SetToken(serializeSession);

                // Giải mã token
                var getUserClaims = DecryptToken(userSession.Token!);

                // Tạo người dùng từ thông tin người dùng
                claimsPrincipal = SetClaimPrincipal(getUserClaims);
            }
            else
            {
                // Nếu token và refresh token rỗng, xóa token khỏi dịch vụ lưu trữ cục bộ
                await localStorageService.RemoveToken();
            }

            // Thông báo cho ứng dụng rằng trạng thái xác thực đã thay đổi
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        // Gán thông tin người dùng vào người dùng
        private static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claims)
        {
            // Nếu email rỗng, trả về người dùng ẩn danh
            if (claims.Email is null) return new ClaimsPrincipal();

            // Tạo người dùng từ thông tin người dùng
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, claims.Id),
                    new(ClaimTypes.Name, claims.Name),
                    new(ClaimTypes.Email, claims.Email),
                    new(ClaimTypes.Role, claims.Role)
                }, "JwtAuth"
            ));
        }

        // Giải mã token
        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            // Nếu token rỗng, trả về thông tin người dùng rỗng
            if (string.IsNullOrEmpty(jwtToken))
            {
                return new CustomUserClaims();
            }

            // Khởi tạo một đối tượng xử lý token
            var handler = new JwtSecurityTokenHandler();

            // Đọc token
            var token = handler.ReadJwtToken(jwtToken);

            // Lấy thông tin người dùng từ token
            var userId = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var name = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var role = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            return new CustomUserClaims(userId!.Value, name!.Value, email!.Value, role!.Value);
        }
    }
}
