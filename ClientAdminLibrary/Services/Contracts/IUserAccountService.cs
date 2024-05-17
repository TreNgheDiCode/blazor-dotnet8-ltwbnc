using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ClientAdminLibrary.Services.Contracts
{
    public interface IUserAccountService
    {
        Task<LoginResponse> SignInAsync(Login user);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken refreshToken);
        Task<WeatherForecast[]> GetWeatherForecasts();
    }
}