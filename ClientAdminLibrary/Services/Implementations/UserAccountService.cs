using BaseLibrary.DTOs;
using BaseLibrary.DTOs.Auth;
using BaseLibrary.Helpers.Client;
using BaseLibrary.Responses;
using ClientAdminLibrary.Services.Contracts;
using System.Net.Http.Json;

namespace ClientAdminLibrary.Services.Implementations
{
    public class UserAccountService(GetHttpClient getHttpClient) : IUserAccountService
    {
        public const string AuthUrl = "api/Authentication";

        public async Task<WeatherForecast[]> GetWeatherForecasts()
        {
            var httpClient = await getHttpClient.GetPrivateHttpClient();
            var result = await httpClient.GetFromJsonAsync<WeatherForecast[]>("api/WeatherForecast");

            return result!;
        }

        public async Task<LoginResponse> SignInAsync(Login user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync(AuthUrl + "/login", user);

            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<LoginResponse>();

                return successResponse ?? new LoginResponse(false, "Login failed");
            }
            else
            {
                var errorResponse = await result.Content.ReadFromJsonAsync<LoginResponse>();

                return errorResponse ?? new LoginResponse(false, "Login failed");
            }
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken refreshToken)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync(AuthUrl + "/refresh-token", refreshToken);

            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<LoginResponse>();

                return successResponse ?? new LoginResponse(false, "Refresh token failed");
            }
            else
            {
                var errorResponse = await result.Content.ReadFromJsonAsync<LoginResponse>();

                return errorResponse ?? new LoginResponse(false, "Refresh token failed");
            }
        }

        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();

            var result = await httpClient.PostAsJsonAsync(AuthUrl + "/register", user);

            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return successResponse ?? new GeneralResponse(false, "Register failed");
            }
            else
            {
                var errorResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return errorResponse ?? new GeneralResponse(false, "Register failed");
            }
        }
    }
}
