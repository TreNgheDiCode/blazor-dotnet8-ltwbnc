using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ServerLibrary.Repositories.Interfaces;
using System.Net;

namespace ServerLibrary.Repositories.Implementations
{
    public class CloudinaryRepo : ICloudinaryRepo
    {
        private static readonly Account account = new("drv0jpgyx", "538589389419466", "RPBArnA_4ksMvxXHRhUoJAxb2SA");
        private readonly Cloudinary cloudinary = new(account);
        
        public async Task<GeneralResponse> DeleteImageAsync(string publicId)
        {
            try
            {
                var result = await cloudinary.DeleteResourcesAsync(new DelResParams()
                {
                    PublicIds = [publicId]
                });

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return new GeneralResponse(true, "Xóa hình ảnh thành công");
                }

                return new GeneralResponse(false, "Xóa hình ảnh thất bại");
            }
            catch (Exception)
            {
                return new GeneralResponse(false, "Xóa hình ảnh thất bại");
            }
        }

        public async Task<ServiceModel<string>> UploadImageAsync(string filePath)
        {
            try
            {
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
