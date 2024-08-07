﻿using BaseLibrary.DTOs.Auth;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IUserAccount
    {
        // Đăng ký
        Task<GeneralResponse> CreateAsync(Register user);
        // Đăng nhập
        Task<LoginResponse> SignInAsync(Login user);
        // Làm mới token
        Task<LoginResponse> RefreshTokenAsync(RefreshToken refreshToken);
    }
}