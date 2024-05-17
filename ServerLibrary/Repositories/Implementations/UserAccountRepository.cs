using BaseLibrary.DTOs;
using BaseLibrary.Models;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ServerLibrary.Repositories.Implementations
{
    public class UserAccountRepository(IOptions<JwtSection> config, AppDbContext appDbContext) : IUserAccount
    {
        // Đăng ký
        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            // Kiểm tra model có rỗng không
            if (user is null) return new GeneralResponse(false, "Vui lòng cung cấp đầy đủ thông tin");

            // Kiểm tra email đã tồn tại chưa
            var checkUser = await FindUserByEmail(user.Email!);
            if (checkUser is not null) return new GeneralResponse(false, "Email đã được sử dụng");

            // Thêm user vào database
            var applicationUser = await AddToDatabase(new ApplicationUser()
            {
                Fullname = user.Fullname,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
            });

            // Kiểm tra, tạo và gán role admin
            var checkAdminRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(r => r.Name!.Equals(Constants.Admin));
            if (checkAdminRole is null)
            {
                var createAdminRole = await AddToDatabase(new SystemRole() { Name = Constants.Admin });
                await AddToDatabase(new UserRole() { RoleId = createAdminRole.Id, UserId = applicationUser.Id });
                return new GeneralResponse(true, "Khởi tạo vai trò quản trị viên thành công");
            }

            // Kiểm tra, tạo và gán role user
            var checkUserRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(r => r.Name!.Equals(Constants.User));
            SystemRole response = new();
            if (checkUserRole is null)
            {
                response = await AddToDatabase(new SystemRole() { Name = Constants.User });
                await AddToDatabase(new UserRole() { RoleId = response.Id, UserId = applicationUser.Id });
                return new GeneralResponse(true, "Khởi tạo vai trò người dùng thành công!");
            }
            else
            {
                await AddToDatabase(new UserRole() { RoleId = checkUserRole.Id, UserId = applicationUser.Id });
            }

            return new GeneralResponse(true, "Đăng ký thành công!");
        }

        // Đăng nhập
        public async Task<LoginResponse> SignInAsync(Login user)
        {
            // Kiểm tra model có rỗng không
            if (user is null) return new LoginResponse(false, "Vui lòng cung cấp đầy đủ thông tin");

            // Kiểm tra user có tồn tại không
            var applicationUser = await FindUserByEmail(user.Email!);
            if (applicationUser is null) return new LoginResponse(false, "Người dùng không tồn tại");

            // Kiểm tra mật khẩu
            if (!BCrypt.Net.BCrypt.Verify(user.Password, applicationUser.Password))
                return new LoginResponse(false, "Thông tin đăng nhập chưa chính xác");

            // Kiểm tra role
            var getUserRole = await appDbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == applicationUser.Id);
            if (getUserRole is null) return new LoginResponse(false, "Quyền hạn không hợp lệ");

            var getRoleName = await appDbContext.SystemRoles.FirstOrDefaultAsync(x => x.Id == getUserRole.RoleId);
            if (getRoleName is null) return new LoginResponse(false, "Không tìm thấy quyền hạn người dùng");

            // Tạo token jwt
            string jwtToken = GenerateToken(applicationUser, getRoleName.Name!);
            // Tạo refresh token
            string refreshToken = GenerateRefreshToken();

            return new LoginResponse(true, "Đăng nhập thành công", jwtToken, refreshToken);
        }

        private string GenerateToken(ApplicationUser user, string role)
        {
            // Token headers & signature
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!)); // Lấy secret key
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // Mã hóa secret key bằng HmacSha256
            // Token payload
            // Claims == Data
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Fullname!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role),
            };
            // Token descriptor
            var token = new JwtSecurityToken(
                issuer: config.Value.Issuer,
                audience: config.Value.Audience,
                claims: userClaims, // Thông tin user
                expires: DateTime.Now.AddDays(1), // Thời gian hết hạn của token
                signingCredentials: credentials
            ); // Full thông tin token

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task<ApplicationUser> FindUserByEmail(string email)
        {
            return await appDbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Email!.ToLower().Equals(email!.ToLower()));
        }
        
        private async Task<T> AddToDatabase<T>(T model)
        {
            var result = appDbContext.Add(model!);
            await appDbContext.SaveChangesAsync();
            return (T)result.Entity;
        }
    }
}