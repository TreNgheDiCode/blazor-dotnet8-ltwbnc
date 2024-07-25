using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface ICloudinaryRepo
    {
        public Task<ServiceModel<string>> UploadImageAsync(string filePath);
        public Task<ServiceModel<string[]>> UploadImagesAsync(string[] filePath);
        public Task<GeneralResponse> DeleteImageAsync(string publicId);
    }
}
