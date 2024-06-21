using BaseLibrary.DTOs.Auth;
using BaseLibrary.Helpers.Client;
using BaseLibrary.Responses;
using ClientUserLibrary.Services.Contracts;
using System.Net.Http.Json;

namespace ClientUserLibrary.Services.Implementations
{
    public class UserAccountService(GetHttpClient getHttpClient) : IUserAccountService
    {
        public const string AuthUrl = "api/Authentication";

        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync(AuthUrl + "/register", user);
            if (!result.IsSuccessStatusCode)
            {
                return new GeneralResponse(false, "Failed to create user");
            }

            return await result.Content.ReadFromJsonAsync<GeneralResponse>();
        }

        public async Task<LoginResponse> SignInAsync(Login user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync(AuthUrl + "/login", user);
            if (!result.IsSuccessStatusCode)
            {
                return new LoginResponse(false, "Failed to sign in");
            }

            return await result.Content.ReadFromJsonAsync<LoginResponse>();
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken refreshToken)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync(AuthUrl + "/refresh-token", refreshToken);
            if (!result.IsSuccessStatusCode)
            {
                return new LoginResponse(false, "Failed to create user");
            }

            return await result.Content.ReadFromJsonAsync<LoginResponse>();
        }
    }
}
