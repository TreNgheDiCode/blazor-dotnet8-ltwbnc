using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IProductRepo
    {
        Task<GeneralResponse> AddProduct(CreateProductDTO product);
        Task<GeneralResponse> AddProductImages(int id, List<ProductImageDTO> productImages);
        Task<GeneralResponse> AddProductOptions(int id, List<ProductOptionDTO> productOptions);
        Task<ServiceModel<ProductItem>> GetProductById(int id);
        Task<ServiceModel<ProductList>> GetProducts(int? page, int? pageSize);
        Task<GeneralResponse> UpdateProduct(int id, UpdateProductDTO product);
        Task<GeneralResponse> UpdateProductImage(int productId, int imageId, string imageUrl);
        Task<GeneralResponse> UpdateProductOption(int productId, int optionId, ProductOptionDTO productOption);
        Task<GeneralResponse> DeleteProduct(int id);
        Task<GeneralResponse> DeleteProductImage(int productId, int imageId);
        Task<GeneralResponse> DeleteProductOption(int productId, int optionId);
    }
}
