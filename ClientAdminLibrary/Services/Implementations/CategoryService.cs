using BaseLibrary.DTOs;
using BaseLibrary.Helpers.Client;
using BaseLibrary.Responses;
using ClientAdminLibrary.Services.Contracts;
using System.Net.Http.Json;

namespace ClientAdminLibrary.Services.Implementations
{
    public class CategoryService(GetHttpClient httpClient) : ICategoryService
    {
        public const string CategoryUrl = "api/categories";

        public async Task<GeneralResponse> CreateCategory(CreateCategoryDTO category)
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.PostAsJsonAsync(CategoryUrl, category);

            // Nếu không có kết quả thì trả về thông báo lỗi
            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return successResponse ?? new GeneralResponse(false, "Thêm danh mục thành công");
            }

            var errorResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

            return errorResponse ?? new GeneralResponse(false, "Thêm danh mục thất bại");
        }

        public async Task<GeneralResponse> DeleteCategory(int id)
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.DeleteAsync($"{CategoryUrl}/{id}");

            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return successResponse ?? new GeneralResponse(false, "Xóa danh mục thành công");
            }

            var errorResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

            return errorResponse ?? new GeneralResponse(false, "Xóa danh mục thất bại");
        }

        public async Task<ServiceModel<CategoryList>> GetCategories()
        {
            var client = httpClient.GetPublicHttpClient();

            var result = await client.GetFromJsonAsync<ServiceModel<CategoryList>>(CategoryUrl);

            // Nếu không có kết quả thì trả về thông báo lỗi
            if (result == null)
            {
                return new ServiceModel<CategoryList>()
                {
                    Data = null,
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }

            return result;
        }

        public async Task<ServiceModel<CategoryItem>> GetCategory(int id)
        {
            var client = httpClient.GetPublicHttpClient();
            var result = await client.GetFromJsonAsync<ServiceModel<CategoryItem>>($"{CategoryUrl}/{id}");

            // Nếu không có kết quả thì trả về thông báo lỗi
            if (result == null)
            {
                return new ServiceModel<CategoryItem>()
                {
                    Data = null,
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }

            return result;
        }

        public async Task<GeneralResponse> UpdateCategory(int id, UpdateCategoryDTO category)
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.PutAsJsonAsync($"{CategoryUrl}/{id}", category);

            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return successResponse ?? new GeneralResponse(false, "Cập nhật danh mục thành công");
            }

            var errorResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

            return errorResponse ?? new GeneralResponse(false, "Cập nhật danh mục thất bại");
        }
    }
}
