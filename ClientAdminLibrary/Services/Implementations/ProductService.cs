using BaseLibrary.DTOs;
using BaseLibrary.Helpers.Client;
using BaseLibrary.Responses;
using ClientAdminLibrary.Services.Contracts;
using System.Net.Http.Json;

namespace ClientAdminLibrary.Services.Implementations
{
    public class ProductService(GetHttpClient httpClient) : IProductService
    {
        public const string ProductUrl = "api/products";

        public Task<GeneralResponse> CreateProduct(CreateProductDTO product)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceModel<ProductItem>> GetProduct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceModel<ProductList>> GetProducts()
        {
            var client = httpClient.GetPublicHttpClient();
            var result = await client.GetFromJsonAsync<ServiceModel<ProductList>>(ProductUrl);

            // Neu result la null thi throw exception
            if (result == null)
            {
                return new ServiceModel<ProductList>()
                {
                    Data = new ProductList(),
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }

            return result;
        }

        public Task<GeneralResponse> UpdateProduct(int id, UpdateProductDTO product)
        {
            throw new NotImplementedException();
        }
    }
}
