using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IUserRepo
    {
        public Task<ServiceModel<UserList>> GetUsers(int? page, int? pageSize);
        public Task<ServiceModel<UserItem>> GetUser(int id);
        public Task<GeneralResponse> UpdateUser(int id, UpdateUserDTO user);
        public Task<GeneralResponse> LockUser(int id);
        public Task<GeneralResponse> DeleteUser(int id);
    }
}
