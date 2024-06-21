using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ClientAdminLibrary.Services.Contracts
{
    public interface IProductService
    {
        Task<ServiceModel<ProductList>> GetProducts();
        Task<ServiceModel<ProductItem>> GetProduct(int id);
        Task<GeneralResponse> CreateProduct(CreateProductDTO product);
        Task<GeneralResponse> UpdateProduct(int id, UpdateProductDTO product);
        Task<GeneralResponse> DeleteProduct(int id);
    }
}
