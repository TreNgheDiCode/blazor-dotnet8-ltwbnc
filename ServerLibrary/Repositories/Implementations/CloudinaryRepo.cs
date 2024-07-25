using BaseLibrary.DTOs;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Repositories.Implementations
{
    public class CloudinaryRepo : ICloudinaryRepo
    {
        public async Task<ServiceModel<string>> UploadImageAsync(string filePath)
        {
            try
            {
                Account account = new("drv0jpgyx", "538589389419466", "RPBArnA_4ksMvxXHRhUoJAxb2SA");
                Cloudinary cloudinary = new(account);

                ImageUploadParams imageUpload = new()
                {
                    File = new FileDescription(filePath),
                    Overwrite = true
                };

                var result = await cloudinary.UploadAsync(imageUpload);

                return new ServiceModel<string>()
                {
                    Data = result.SecureUri.AbsoluteUri,
                    Message = "Đăng tải hình ảnh thành công",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceModel<string>()
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<ServiceModel<string[]>> UploadImagesAsync(string[] filePath)
        {
            try
            {
                Account account = new("drv0jpgyx", "538589389419466", "RPBArnA_4ksMvxXHRhUoJAxb2SA");
                Cloudinary cloudinary = new(account);

                List<string> imageUrls = [];

                foreach (var path in filePath)
                {
                    ImageUploadParams imageUpload = new()
                    {
                        File = new FileDescription(path),
                        Overwrite = true
                    };

                    var result = await cloudinary.UploadAsync(imageUpload);
                    imageUrls.Add(result.SecureUri.AbsoluteUri);
                }

                return new ServiceModel<string[]>()
                {
                    Data = imageUrls.ToArray(),
                    Message = "Đăng tải hình ảnh thành công",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceModel<string[]>()
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
