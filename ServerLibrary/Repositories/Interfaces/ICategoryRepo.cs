using BaseLibrary.DTOs;
using BaseLibrary.Models.Products;
namespace ServerLibrary.Repositories.Interfaces
{
    public interface ICategoryRepo
    {
        public Task<ServiceModel<Category>> GetCategory(int id);
        public Task<ServiceModel<Category>> GetCategories();
        public Task<ServiceModel<Category>> AddCategory(Category category);
        public Task<ServiceModel<Category>> UpdateCategory(int id,string? name);
        public Task<ServiceModel<Category>> DeleteCategory(int id);
   
    }
}
