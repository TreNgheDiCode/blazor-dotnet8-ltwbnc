using BaseLibrary.DTOs;
using BaseLibrary.Models;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Repositories.Implementations
{
    public class UserRepo(AppDbContext context) : IUserRepo
    {
        // Lấy thông tin người dùng hiện đang đăng nhập

        public async Task<GeneralResponse> DeleteUser(int id)
        {
            // Lấy user theo id cùng vai trò
            var userWithRole = await context.ApplicationUsers
                .Where(u => u.Id == id)
                .Join(context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                .Join(context.SystemRoles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur.u, r })
                .FirstOrDefaultAsync();

            // Nếu không có user nào, trả về lỗi
            if (userWithRole is null)
            {
                return new GeneralResponse(false, "Người dùng không tồn tại");
            }

            // Kiểm tra nếu User là tài khoản Admin, không cho phép xóa
            if (userWithRole.r.Name == "Admin")
            {
                return new GeneralResponse(false, "Không thể xóa tài khoản với quyền hạn Admin");
            }

            // Xóa user
            context.ApplicationUsers.Remove(userWithRole.u);

            // Lưu thay đổi
            await context.SaveChangesAsync();

            // Trả về thông báo thành công
            return new GeneralResponse(true, "Xóa người dùng thành công");
        }

        public async Task<ServiceModel<UserItem>> GetUser(int id)
        {
            // Lấy user với vai trò theo id user
            var userWithRole = await context.ApplicationUsers
                .Where(u => u.Id == id)
                .Join(context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                .Join(context.SystemRoles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur.u, r })
                .Select(u => new
                {
                    u.u.Id,
                    u.u.Fullname,
                    u.u.Email,
                    u.u.PhoneNumber,
                    u.u.Address!.AddressDetail,
                    WardName = u.u.Address.Ward!.FullName,
                    DistrictName = u.u.Address.District!.FullName,
                    ProvinceName = u.u.Address.Province!.FullName,
                    u.u.Photo,
                    u.u.Other,
                    u.u.CreatedAt,
                    u.u.IsLocked,
                    Role = u.r.Name
                })
                .FirstOrDefaultAsync();

            // Nếu không có user nào, trả về lỗi
            if (userWithRole is null)
            {
                return new ServiceModel<UserItem>
                {
                    Data = null,
                    Message = "Người dùng không tồn tại",
                    Success = false
                };
            }

            // Tạo UserItem từ user với vai trò
            UserItem user = new()
            {
                Id = userWithRole.Id,
                Fullname = userWithRole.Fullname,
                Email = userWithRole.Email,
                PhoneNumber = userWithRole.PhoneNumber,
                Address = userWithRole.AddressDetail != null ? $"{userWithRole.AddressDetail}, {userWithRole.WardName}, {userWithRole.DistrictName}, {userWithRole.ProvinceName}" : null,
                Photo = userWithRole.Photo,
                Other = userWithRole.Other,
                CreatedAt = userWithRole.CreatedAt,
                IsLocked = userWithRole.IsLocked,
                Role = userWithRole.Role
            };

            // Trả về user với vai trò
            ServiceModel<UserItem> response = new()
            {
                Data = user,
                Message = "Lấy thông tin người dùng thành công",
                Success = true
            };

            return response;
        }

        public async Task<ServiceModel<UserList>> GetUsers(int? page, int? pageSize)
        {
            // Giá trị mặc định của page và pageSize
            int currentPage = page ?? 1; // Nếu page là null, trả về trang đầu tiên
            int currentPageSize = pageSize ?? await context.ApplicationUsers.CountAsync(); // Nếu pageSize là null, trả về tất cả

            // Tính tổng số lượng user
            int totalCount = await context.ApplicationUsers.CountAsync();

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            // Lấy danh sách user với vai trò
            var usersWithRoles = await (from user in context.ApplicationUsers
                                        join userRole in context.UserRoles on user.Id equals userRole.UserId
                                        join role in context.SystemRoles on userRole.RoleId equals role.Id
                                        select new
                                        {
                                            user.Id,
                                            user.Fullname,
                                            user.Email,
                                            user.PhoneNumber,
                                            user.Address!.AddressDetail,
                                            WardName = user.Address.Ward!.FullName,
                                            DistrictName = user.Address.District!.FullName,
                                            ProvinceName = user.Address.Province!.FullName,
                                            user.Photo,
                                            user.Other,
                                            user.CreatedAt,
                                            user.IsLocked,
                                            Role = role.Name
                                        })
                                        .Skip((currentPage - 1) * currentPageSize)
                                        .Take(currentPageSize)
                                        .ToListAsync();

            // Nếu không có user nào, trả về lỗi
            if (usersWithRoles.Count == 0)
            {
                return new ServiceModel<UserList>
                {
                    Data = null,
                    Message = "Danh sách người dùng trống",
                    Success = false
                };
            }

            // Tạo danh sách UserItem từ danh sách user với vai trò
            List<UserItem> users = usersWithRoles.Select(u => new UserItem
            {
                Id = u.Id,
                Fullname = u.Fullname,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Address = u.AddressDetail != null ? $"{u.AddressDetail}, {u.WardName}, {u.DistrictName}, {u.ProvinceName}" : null,
                Photo = u.Photo,
                Other = u.Other,
                CreatedAt = u.CreatedAt,
                IsLocked = u.IsLocked,
                Role = u.Role
            }).ToList();

            // Tạo UserList từ danh sách UserItem
            UserList userList = new()
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = currentPageSize,
                Users = users
            };

            // Trả về danh sách user với vai trò
            ServiceModel<UserList> response = new()
            {
                Data = userList,
                Message = "Lấy danh sách người dùng thành công",
                Success = true
            };

            return response;
        }

        public async Task<GeneralResponse> UpdateUser(int id, UpdateUserDTO user)
        {
            // Lấy user theo id cùng vai trò
            var userWithRole = await context.ApplicationUsers
                .Where(u => u.Id == id)
                .Join(context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                .Join(context.SystemRoles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur.u, r })
                .FirstOrDefaultAsync();

            // Nếu không có user nào, trả về lỗi
            if (userWithRole is null)
            {
                return new GeneralResponse(false, "Người dùng không tồn tại");
            }

            // TODO: Sử dụng hệ thống lấy token, kiểm tra role user và ngăn chặn tài khoản cập nhật vai trò

            // Nếu giá trị vai trò thay đổi, kiểm tra xem vai trò có tồn tại không
            if (user.Role != null && !context.SystemRoles.Any(r => r.Name == user.Role))
            {
                return new GeneralResponse(false, "Vai trò không tồn tại");
            }

            // Nếu mã phường/xã/thị trấn không hợp lệ, trả về lỗi
            if (user.WardId != null && !context.Wards.Any(w => w.Code == user.WardId))
            {
                return new GeneralResponse(false, "Mã phường/xã/thị trấn không hợp lệ");
            }

            // Nếu mã quận/huyện không hợp lệ, trả về lỗi
            if (user.DistrictId != null && !context.Districts.Any(d => d.Code == user.DistrictId))
            {
                return new GeneralResponse(false, "Mã quận/huyện không hợp lệ");
            }

            // Nếu mã tỉnh/thành phố không hợp lệ, trả về lỗi
            if (user.ProvinceId != null && !context.Provinces.Any(p => p.Code == user.ProvinceId))
            {
                return new GeneralResponse(false, "Mã tỉnh/thành phố không hợp lệ");
            }

            // Cập nhật thông tin user
            userWithRole.u.Fullname = user.Fullname;
            userWithRole.u.Email = user.Email;
            userWithRole.u.PhoneNumber = user.PhoneNumber;
            // Tìm địa chỉ của user hiện tại và cập nhật, nếu không tìm thấy thì tạo mới
            Address address = await context.Addresses.FindAsync(userWithRole.u.AddressId);
            if (address is null)
            {
                address = new Address
                {
                    AddressDetail = user.Address,
                    WardId = user.WardId,
                    DistrictId = user.DistrictId,
                    ProvinceId = user.ProvinceId
                };
                context.Addresses.Add(address);
            }
            else
            {
                address.AddressDetail = user.Address;
                address.WardId = user.WardId;
                address.DistrictId = user.DistrictId;
                address.ProvinceId = user.ProvinceId;
            }
            userWithRole.u.Photo = user.Photo;
            userWithRole.u.Other = user.Other;
            userWithRole.u.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            userWithRole.u.IsLocked = user.IsLocked;

            // Nếu giá trị vai trò thay đổi, cập nhật vai trò
            if (user.Role != null)
            {
                // Lấy id của vai trò mới
                int roleId = await context.SystemRoles
                    .Where(r => r.Name == user.Role)
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

                // Cập nhật vai trò trong bảng vai trò người dùng
                var userRole = await context.UserRoles
                    .Where(ur => ur.UserId == userWithRole.u.Id)
                    .FirstOrDefaultAsync();

                if (roleId == 0) {
                    return new GeneralResponse(false, "Vai trò không hợp lệ");
                }

                if (userRole is null)
                {
                    userRole = new UserRole
                    {
                        UserId = userWithRole.u.Id,
                        RoleId = roleId
                    };
                    context.UserRoles.Add(userRole);
                }
                else
                {
                    userRole.RoleId = roleId;
                }

                userRole.RoleId = roleId;
            }

            // Lưu thay đổi
            await context.SaveChangesAsync();

            // Trả về thông báo thành công
            return new GeneralResponse(true, "Cập nhật thông tin người dùng thành công");
        }
    }
}
