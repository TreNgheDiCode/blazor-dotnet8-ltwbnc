﻿using BaseLibrary.DTOs;
using BaseLibrary.DTOs.Auth;
using BaseLibrary.Responses;

namespace ClientAdminLibrary.Services.Contracts
{
    public interface IUserService
    {
        Task<ServiceModel<UserList>> GetUsers();
        Task<ServiceModel<UserItem>> GetUser(int id);
        Task<GeneralResponse> CreateUser(Register user);
        Task<GeneralResponse> UpdateUser(int id, UpdateUserDTO user);
        Task<GeneralResponse> DeleteUser(int id);
    }
}