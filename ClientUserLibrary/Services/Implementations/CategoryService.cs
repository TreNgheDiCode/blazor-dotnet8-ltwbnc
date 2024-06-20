using BaseLibrary.DTOs;
using BaseLibrary.Helpers.Client;
using ClientUserLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientUserLibrary.Services.Implementations
{
    public class CategoryService(GetHttpClient httpClient) : ICategoryService
    {
        private const string CategoryUrl = "api/categories";
        public async Task<ServiceModel<CategoryList>> GetCategories(int? page, int? pageSize)
        {
            var client = httpClient.GetPublicHttpClient();
            var result = await client.GetFromJsonAsync<ServiceModel<CategoryList>>(CategoryUrl);
            if (result == null)
            {
                return new ServiceModel<CategoryList>()
                {
                    Data = new CategoryList(),
                    Message = "Failed to get categories",
                    Success = false
                };
            }

            return result!;
        }
    }
}
