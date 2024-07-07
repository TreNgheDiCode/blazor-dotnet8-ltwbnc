using BaseLibrary.DTOs;
using BaseLibrary.Helpers.Client;
using BaseLibrary.Responses;
using ClientUserLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientUserLibrary.Services.Implementations
{
    public class ProductsService(GetHttpClient httpClient) : IProductsService
    {
        public const string ProductUrl = "api/products";

        public async Task<ServiceModel<ProductItem>> GetProduct(int id)
        {
            var client = httpClient.GetPublicHttpClient();
            var result = await client.GetFromJsonAsync<ServiceModel<ProductItem>>($"./api/products/{id}");
            if(result == null)
            {
                return new ServiceModel<ProductItem>()
                {
                    Data = new ProductItem(),
                    Message = "Failed to get products",
                    Success = false
                };
            }
            return result;
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
                    Message = "Failed to get products",
                    Success = false
                };
            }

            return result!;
        }

    }
}
