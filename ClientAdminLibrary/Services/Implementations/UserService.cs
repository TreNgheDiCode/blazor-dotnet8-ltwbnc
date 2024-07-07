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
        public const string ProductUrl = "api/users";

        public Task<GeneralResponse> CreateUser(Register user)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceModel<UserItem>> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceModel<UserList>> GetUsers()
        {
            var client = await httpClient.GetPrivateHttpClient();
            var result = await client.GetFromJsonAsync<ServiceModel<UserList>>(ProductUrl);

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

        public Task<GeneralResponse> UpdateUser(int id, UpdateUserDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
