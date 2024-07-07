using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ClientAdminLibrary.Services.Contracts
{
    public interface ICategoryService
    {
        Task<ServiceModel<CategoryList>> GetCategories();
        Task<ServiceModel<CategoryItem>> GetCategory(int id);
        Task<GeneralResponse> CreateCategory(CreateCategoryDTO category);
        Task<GeneralResponse> UpdateCategory(int id, UpdateCategoryDTO category);
        Task<GeneralResponse> DeleteCategory(int id);
    }
}
