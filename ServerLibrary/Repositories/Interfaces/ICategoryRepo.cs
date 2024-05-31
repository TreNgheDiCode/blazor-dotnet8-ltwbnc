using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.DTOs;
using BaseLibrary.Models.Products;
namespace ServerLibrary.Repositories.Interfaces
{
    public interface ICategoryRepo
    {
        public Task<ServiceCategory> GetCategory(int id);
        public Task<ServiceCategory> GetCategories();
        public Task<ServiceCategory> AddCategory(Category category);
        public Task<ServiceCategory> UpdateCategory(int id,string? name);
        public Task<ServiceCategory> DeleteCategory(int id);
   
    }
}
