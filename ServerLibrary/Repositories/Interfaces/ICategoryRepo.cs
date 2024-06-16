using BaseLibrary.DTOs;
using BaseLibrary.Models.Products;
using BaseLibrary.Responses;
namespace ServerLibrary.Repositories.Interfaces
{
    public interface ICategoryRepo
    {
        public Task<ServiceModel<CategoryItem>> GetCategory(int id);
        public Task<ServiceModel<CategoryList>> GetCategories(int? page, int? pageSize);
        public Task<GeneralResponse> AddCategory(CreateCategoryDTO category);
        public Task<GeneralResponse> UpdateCategory(int id, UpdateCategoryDTO category);
        public Task<GeneralResponse> DeleteCategory(int id);
   
    }
}
