using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ClientUserLibrary.Services.Contracts
{
    public interface IUserAccountService
    {
        Task<GeneralResponse> CreateAsync(Register user);
        Task<LoginResponse> SignInAsync(Login user);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken refreshToken);
    }
}