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

        public async Task<ServiceModel<string[]>> UploadImagesAsync(string[] filePaths)
        {
            var client = httpClient.GetPublicHttpClient();
            var content = new MultipartFormDataContent();

            foreach (var path in filePaths)
            {
                try
                {
                    using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    var fileInfo = new FileInfo(path);
                    var fileContent = new StreamContent(fileStream);
                    content.Add(fileContent, "file", fileInfo.Name);
                }
                catch (Exception ex)
                {
                    return new ServiceModel<string[]>()
                    {
                        Data = null,
                        Message = $"Error uploading file {path}: {ex.Message}",
                        Success = false
                    };
                }
            }

            var result = await client.PostAsync($"{CloudinaryUrl}/multiple", content);

            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<ServiceModel<string[]>>();
                return successResponse ?? new ServiceModel<string[]>()
                {
                    Data = null,
                    Message = "Đăng tải hình ảnh thành công",
                    Success = true
                };
            }
            else
            {
                var errorResponse = await result.Content.ReadFromJsonAsync<ServiceModel<string[]>>();
                return errorResponse ?? new ServiceModel<string[]>()
                {
                    Data = null,
                    Message = "Đăng tải hình ảnh thất bại",
                    Success = false
                };
            }
        }

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
