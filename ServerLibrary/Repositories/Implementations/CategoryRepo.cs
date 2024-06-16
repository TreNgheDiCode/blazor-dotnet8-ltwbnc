using Azure.Core;
using BaseLibrary.DTOs;
using BaseLibrary.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Repositories.Implementations
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext appContext;

        public CategoryRepo(AppDbContext appContext)
        {
            this.appContext = appContext;
        }

        public async Task<ServiceModel<Category>> AddCategory(Category Newcategory)
        {
            var response = new ServiceModel<Category>();
            if (Newcategory != null)
            {
                try
                {
                    appContext.Categories.Add(Newcategory);
                    await appContext.SaveChangesAsync();
                    response.Data = Newcategory;
                    response.Message = "Add Category Successfully!";
                    response.Success = true;
                    return response;
                }
                catch (Exception ex)
                {
                    response.Message = ex.Message.ToString();
                    response.Success = false;
                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Sorry New Category Opject is Empty";
                response.Data = null;
                return response;
            }
        }

        public async Task<ServiceModel<Category>> GetCategories()
        {
            var response = new ServiceModel<Category>();
            try
            {
                var categories = await appContext.Categories.ToListAsync();
                if (categories != null)
                {
                    response.Data = null;
                    response.Success = true;
                    response.Message = "Found!";
                    return response;
                }
                else
                {
                    response.Data = null;
                    response.Success = false;
                    response.Message = "Category not found!";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message.ToString();
                return response;
            }
        }

        public async Task<ServiceModel<Category>> GetCategory(int Id)
        {
            var response = new ServiceModel<Category>();
            if (Id != 0)
            {
                try
                {
                    var category = await appContext.Categories.SingleOrDefaultAsync(p => p.Id == Id);
                    if (category != null)
                    {
                        response.Success = true;
                        response.Message = "Product Found";
                        response.Data = category;
                        return response;

                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Sorry! category you are looking for doesn't exist!";
                        response.Data = null;
                        return response;

                    }

                }
                catch (Exception ex)
                {
                    response.Success = !false;
                    response.Message = ex.Message.ToString();
                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Sorry new category  object is empty!";
                response.Data = null;
                return response;
            }
        }

        public async Task<ServiceModel<Category>> UpdateCategory(int Id, string? name)
        {
            var response = new ServiceModel<Category>();

            // Fetch the category
            var category = await GetCategory(Id);
            if (category == null)
            {
                // Return an error response if category does not exist
                response.Success = false;
                response.Message = "Category not found.";
                return response;
            }

            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    category.Data.Name = name;
                    await appContext.SaveChangesAsync();

                    response.Success = true;
                    response.Message = "Category updated!";
                    return response;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = ex.Message;
                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Couldn't update";
                return response;
            }
        }

        public async Task<ServiceModel<Category>> DeleteCategory(int Id)
        {
            var response = new ServiceModel<Category>();
            var cate = await GetCategory(Id);
            if (cate.Data != null)
            {
                appContext.Categories.Remove(cate.Data);
                await appContext.SaveChangesAsync();
                response.Message = "Category Deleted!";
                return response;
            }
            else
            {
                response.Message = cate.Message;
            }
            return response;
        }
    }
}
