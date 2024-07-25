using BaseLibrary.DTOs;
using BaseLibrary.DTOs.Auth;
using BaseLibrary.Responses;

namespace ClientAdminLibrary.Services.Contracts
{
    public interface IUserService
    {
        Task<ServiceModel<UserList>> GetUsers();
        Task<ServiceModel<UserItem>> GetUser(int id);
        Task<GeneralResponse> UpdateUser(int id, UpdateUserDTO user);
        Task<GeneralResponse> LockUser(int id);
        Task<GeneralResponse> DeleteUser(int id);
    }
}
