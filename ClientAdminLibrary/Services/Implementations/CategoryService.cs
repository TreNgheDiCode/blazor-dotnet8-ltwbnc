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

        public Task<GeneralResponse> CreateCategory(CreateCategoryDTO category)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> DeleteCategory(int id)
        {
            throw new NotImplementedException();
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

        public Task<ServiceModel<CategoryItem>> GetCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> UpdateCategory(int id, UpdateCategoryDTO category)
        {
            throw new NotImplementedException();
        }
    }
}
