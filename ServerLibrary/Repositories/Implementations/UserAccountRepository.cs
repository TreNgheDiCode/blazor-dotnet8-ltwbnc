using BaseLibrary.DTOs.Auth;
using BaseLibrary.Models;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Interfaces;
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

            // Kiểm tra phuong/xã/thị trấn có tồn tại không
            var checkWard = await appDbContext.Wards.FirstOrDefaultAsync(x => x.Code == user.WardId);
            if (checkWard is null) return new GeneralResponse(false, "Phường/xã/thị trấn không tồn tại");

            // Kiểm tra quận/huyện có tồn tại không
            var checkDistrict = await appDbContext.Districts.FirstOrDefaultAsync(x => x.Code == user.DistrictId);
            if (checkDistrict is null) return new GeneralResponse(false, "Quận/huyện không tồn tại");

            // Kiểm tra thành phố/tỉnh có tồn tại không
            var checkProvince = await appDbContext.Provinces.FirstOrDefaultAsync(x => x.Code == user.ProvinceId);
            if (checkProvince is null) return new GeneralResponse(false, "Thành phố/tỉnh không tồn tại");

            // Thêm user vào database
            var applicationUser = await AddToDatabase(new ApplicationUser()
            {
                Fullname = user.Fullname,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                PhoneNumber = user.PhoneNumber,
                Address = new Address()
                {
                    AddressDetail = user.Address,
                    WardId = user.WardId,
                    DistrictId = user.DistrictId,
                    ProvinceId = user.ProvinceId
                },
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                Photo = user.Photo,
                Other = user.Other,
                IsLocked = false

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
            // Kiểm tra vai trò có tồn tại không
            else if (user.Role is not null)
            {
                var checkRole = await appDbContext.SystemRoles.FirstOrDefaultAsync(x => x.Name!.Equals(user.Role));

                // Trả về thông báo lỗi
                if (checkRole is null) return new GeneralResponse(false, "Vai trò không tồn tại");
                else
                {
                    await AddToDatabase(new UserRole() { RoleId = checkRole.Id, UserId = applicationUser.Id });
                }
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
            var getUserRole = await FindUserRole(applicationUser.Id);
            if (getUserRole is null) return new LoginResponse(false, "Quyền hạn không hợp lệ");

            var getRoleName = await FindRoleName(getUserRole.RoleId);
            if (getRoleName is null) return new LoginResponse(false, "Không tìm thấy quyền hạn người dùng");

            // Nếu tài khoản bị khóa
            if (applicationUser.IsLocked is not null && applicationUser.IsLocked == true)
                return new LoginResponse(false, "Tài khoản đã bị khóa");

            // Tạo token jwt
            string jwtToken = GenerateToken(applicationUser, getRoleName.Name!);
            // Tạo refresh token
            string refreshToken = GenerateRefreshToken();

            // Kiểm tra refresh token
            var checkRefreshToken = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(x => x.UserId == applicationUser.Id);

            if (checkRefreshToken is not null)
            {
                checkRefreshToken.Token = refreshToken;
                await appDbContext.SaveChangesAsync();
            }
            else
            {
                await AddToDatabase(new RefreshTokenInfo() { Token = refreshToken, UserId = applicationUser.Id });
            }

            return new LoginResponse(true, "Đăng nhập thành công", jwtToken, refreshToken);
        }

        // Làm mới token
        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken refreshToken)
        {
            // Kiểm tra model có rỗng không
            if (refreshToken is null) return new LoginResponse(false, "Vui lòng cung cấp đầy đủ thông tin");

            // Kiểm tra token
            var findToken = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(x => x.Token!.Equals(refreshToken.Token));
            if (findToken is null) return new LoginResponse(false, "Không tìm thấy token");

            // Kiểm tra user
            var user = await appDbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == findToken.UserId);
            if (user is null) return new LoginResponse(false, "Người dùng không tồn tại");

            // Kiểm tra role
            var getUserRole = await FindUserRole(user.Id);
            if (getUserRole is null) return new LoginResponse(false, "Quyền hạn không hợp lệ");

            var getRoleName = await FindRoleName(getUserRole.RoleId);
            if (getRoleName is null) return new LoginResponse(false, "Không tìm thấy quyền hạn người dùng");

            // Tạo token jwt
            string jwtToken = GenerateToken(user, getRoleName.Name!);
            // Tạo refresh token mới
            string newRefreshToken = GenerateRefreshToken();

            // Người dùng chưa đăng nhập - kiểm tra refresh token
            var checkRefreshToken = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (checkRefreshToken is null) return new LoginResponse(false, "Phiên đăng nhập không tồn tại");

            // Người dùng đã đăng nhập - cập nhật refresh token
            checkRefreshToken.Token = newRefreshToken;
            await appDbContext.SaveChangesAsync();

            return new LoginResponse(true, "Làm mới token thành công", jwtToken, newRefreshToken);
        }

        private async Task<UserRole> FindUserRole(int userId)
        {
            return await appDbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        private async Task<SystemRole> FindRoleName(int roleId)
        {
            return await appDbContext.SystemRoles.FirstOrDefaultAsync(x => x.Id == roleId);
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