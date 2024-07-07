using BaseLibrary.DTOs;
using BaseLibrary.DTOs.Auth;
using BaseLibrary.Helpers.Client;
using ClientUserLibrary.Services.Contracts;
using System.Net;
using System.Net.Http.Headers;

namespace ClientAdminLibrary.Helpers
{
    public class CustomHttpHandler(GetHttpClient getHttpClient, LocalStorageService localStorageService, IUserAccountService userAccountService) : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Kiểm tra xem request có phải là login hoặc refresh-token không
            bool loginUrl = request.RequestUri!.AbsoluteUri.Contains("login");
            bool refreshToken = request.RequestUri!.AbsoluteUri.Contains("refresh-token");

            // Nếu request là login hoặc refresh-token, gửi request
            if (loginUrl || refreshToken)
            {
                return await base.SendAsync(request, cancellationToken);
            }


            var res = await base.SendAsync(request, cancellationToken);
            // Nếu trả về là Unauthorized
            if (res.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Lấy token từ local storage
                var stringToken = await localStorageService.GetToken();
                if (stringToken == null) return res;

                // Lấy token từ request header
                string token = string.Empty;
                try
                {
                    token = request.Headers.Authorization!.Parameter!;
                }
                catch { }

                // Deserialize token
                var deserializeToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
                if (deserializeToken == null) return res;

                // If the token is empty, set the token from the local storage
                if (string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", deserializeToken.Token);
                    return await base.SendAsync(request, cancellationToken);
                }

                // If the token is expire, get a new token
                var newJwtToken = await GetRefreshTokenAsync(deserializeToken.RefreshToken!);
                if (string.IsNullOrEmpty(newJwtToken)) return res;

                // Set the new token
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newJwtToken);
                return await base.SendAsync(request, cancellationToken);
            }
            return res;
        }

        private async Task<string> GetRefreshTokenAsync(string refreshToken)
        {
            // Get a new token
            var result = await userAccountService.RefreshTokenAsync(new RefreshToken() { Token = refreshToken });
            // If the result is empty, return an empty string
            string serializedToken = Serializations.SerializeObj(new UserSession() { Token = result.Token, RefreshToken = result.RefreshToken });
            await localStorageService.SetToken(serializedToken);

            return result.Token;
        }
    }
}