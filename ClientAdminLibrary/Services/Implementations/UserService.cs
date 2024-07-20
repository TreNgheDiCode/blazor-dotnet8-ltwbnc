using BaseLibrary.DTOs;
using BaseLibrary.DTOs.Auth;
using BaseLibrary.Helpers.Client;
using BaseLibrary.Responses;
using ClientAdminLibrary.Services.Contracts;
using System.Net.Http.Json;

namespace ClientAdminLibrary.Services.Implementations
{
    public class UserService(GetHttpClient httpClient) : IUserService
    {
        public const string UserUrl = "api/users";


        public async Task<GeneralResponse> DeleteUser(int id)
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.DeleteAsync($"{UserUrl}/{id}");

            if (result.IsSuccessStatusCode)
            {
                return new GeneralResponse(true, "Xóa người dùng thành công");
            }

            // Get the response message
            var response = await result.Content.ReadAsStringAsync();

            return new GeneralResponse(false, response);
        }

        public async Task<ServiceModel<UserItem>> GetUser(int id)
        {
            var client = await httpClient.GetPrivateHttpClient();
            var result = await client.GetFromJsonAsync<ServiceModel<UserItem>>($"{UserUrl}/{id}");

            // Nếu không có kết quả thì trả về thông báo lỗi
            if (result == null)
            {
                return new ServiceModel<UserItem>()
                {
                    Data = null,
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }

            return result;
        }

        public async Task<ServiceModel<UserList>> GetUsers()
        {
            var client = await httpClient.GetPrivateHttpClient();
            var result = await client.GetFromJsonAsync<ServiceModel<UserList>>(UserUrl);

            // Nếu không có kết quả thì trả về thông báo lỗi
            if (result == null)
            {
                return new ServiceModel<UserList>()
                {
                    Data = null,
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }

            return result;
        }

        public async Task<GeneralResponse> UpdateUser(int id, UpdateUserDTO user)
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.PutAsJsonAsync($"{UserUrl}/{id}", user);

            if (result.IsSuccessStatusCode)
            {
                return new GeneralResponse(true, "Cập nhật người dùng thành công");
            }

            return new GeneralResponse(false, "Cập nhật người dùng thất bại");
        }
    }
}
