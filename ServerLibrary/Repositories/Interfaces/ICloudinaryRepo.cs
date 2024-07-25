using BaseLibrary.DTOs;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface ICloudinaryRepo
    {
        public Task<ServiceModel<string>> UploadImageAsync(string filePath);
        public Task<ServiceModel<string[]>> UploadImagesAsync(string[] filePath);
    }
}
