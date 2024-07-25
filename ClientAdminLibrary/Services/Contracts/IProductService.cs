using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ClientAdminLibrary.Services.Contracts
{
    public interface IProductService
    {
        Task<ServiceModel<ProductList>> GetProducts();
        Task<ServiceModel<ProductItem>> GetProduct(int id);
        Task<GeneralResponse> CreateProduct(CreateProductDTO product);
        Task<GeneralResponse> AddProductImage(int id, List<ProductImageDTO> productImages);
        Task<GeneralResponse> UpdateProduct(int id, UpdateProductDTO product);
        Task<GeneralResponse> FlashSale(int id);
        Task<GeneralResponse> DeleteProduct(int id);
        Task<GeneralResponse> DeleteProductImage(int productId, int imageId);
        Task<GeneralResponse> DeleteProductOption(int productId, int optionId);
    }
}
