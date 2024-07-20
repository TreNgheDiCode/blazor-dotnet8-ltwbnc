using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Http;

namespace ClientAdminLibrary.Services.Contracts
{
    public interface ICloudinaryService
    {
        Task<ServiceModel<string>> UploadImageAsync(Stream stream, string fileName);
    }
}
