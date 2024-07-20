using BaseLibrary.DTOs;
using BaseLibrary.Helpers.Client;
using ClientAdminLibrary.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace ClientAdminLibrary.Services.Implementations
{
    public class CloudinaryService(GetHttpClient httpClient) : ICloudinaryService
    {
        public const string CloudinaryUrl = "api/cloudinary";

        public async Task<ServiceModel<string>> UploadImageAsync(Stream stream, string fileName)
        {
            var client = httpClient.GetPublicHttpClient();

            var content = new MultipartFormDataContent
            {
                { new StreamContent(stream), "file", fileName }
            };

            var result = await client.PostAsync(CloudinaryUrl, content);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<ServiceModel<string>>();
            }
            else
            {
                return new ServiceModel<string>()
                {
                    Data = null,
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }
        }
    }
}
