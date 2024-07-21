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

        public async Task<GeneralResponse> CreateProduct(CreateProductDTO product)
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.PostAsJsonAsync(ProductUrl, product);

            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return successResponse ?? new GeneralResponse(false, "Thêm sản phẩm thành công");
            }
            else
            {
                var errorResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return errorResponse ?? new GeneralResponse(false, "Thêm sản phẩm thất bại");
            }
        }

        public async Task<GeneralResponse> DeleteProduct(int id)
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.DeleteAsync($"{ProductUrl}/{id}");

            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return successResponse ?? new GeneralResponse(false, "Xóa sản phẩm thành công");
            }
            else
            {
                var errorResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return errorResponse ?? new GeneralResponse(false, "Xóa sản phẩm thất bại");
            }
        }

        public async Task<GeneralResponse> FlashSale(int id)
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.PutAsync($"{ProductUrl}/{id}/flash-sale", null);

            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return successResponse ?? new GeneralResponse(false, "Khuyến mãi thành công");
            }
            else
            {
                var errorResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return errorResponse ?? new GeneralResponse(false, "Khuyến mãi thất bại");
            }
        }

        public async Task<ServiceModel<ProductItem>> GetProduct(int id)
        {
            var client = httpClient.GetPublicHttpClient();
            var result = await client.GetFromJsonAsync<ServiceModel<ProductItem>>($"{ProductUrl}/{id}");

            // Neu result la null thi throw exception
            if (result == null)
            {
                return new ServiceModel<ProductItem>()
                {
                    Data = new ProductItem(),
                    Message = "Lỗi máy chủ",
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
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }

            return result;
        }

        public async Task<GeneralResponse> UpdateProduct(int id, UpdateProductDTO product)
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.PutAsJsonAsync($"{ProductUrl}/{id}", product);

            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return successResponse ?? new GeneralResponse(false, "Cập nhật sản phẩm thành công");
            }
            else
            {
                var errorResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return errorResponse ?? new GeneralResponse(false, "Cập nhật sản phẩm thất bại");
            }
        }
    }
}
