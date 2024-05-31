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

namespace ServerLibrary.Repositories.Emplementations
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext appContext;

        public CategoryRepo(AppDbContext appContext)
        {
            this.appContext = appContext;
        }

        public async Task<ServiceCategory> AddCategory(Category Newcategory)
        {
            var response=new ServiceCategory();
            if (Newcategory != null)
            {
                try
                {
                    appContext.Categories.Add(Newcategory);
                    await appContext.SaveChangesAsync();
                    response.SingleCategory = Newcategory;
                    response.Message = "Add Category Successfully!";
                    response.Success = true;
                    return response;
                } catch (Exception ex)
                {
                    response.Message = ex.Message.ToString();
                    response.Success = false;
                    return response;
                }
            }else
            {
                response.Success= false;
                response.Message = "Sorry New Category Opject is Empty";
                response.SingleCategory= null;
                return response;
            }
        }

        public async Task<ServiceCategory> GetCategories()
        {
            var response = new ServiceCategory();
            try
            {
                var categories = await appContext.Categories.ToListAsync();
                if(categories!=null)
                {
                    response.ListCategory = categories;
                    response.Success = true;
                    response.Message = "Found!";
                    return response;
                }
                else
                {
                    response.ListCategory = null;
                    response.Success = false;
                    response.Message = "Category not found!";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Success= false;
                response.Message = ex.Message.ToString();
                return response;
            }
        }

        public async Task<ServiceCategory> GetCategory(int IdCategory)
        {
            var response = new ServiceCategory();
            if (IdCategory != 0)
            {
                try
                {
                    var category =await appContext.Categories.SingleOrDefaultAsync(p=>p.Id== IdCategory);
                    if(category != null)
                    {
                        response.Success=true;
                        response.Message = "Product Found";
                        response.SingleCategory = category;
                        return response;

                    }else
                    {
                        response.Success=false;
                        response.Message = "Sorry! category you are looking for doesn't exist!";
                        response.SingleCategory = null;
                        return response;

                    }

                }catch (Exception ex)
                {
                    response.Success=! false;
                    response.Message= ex.Message.ToString();
                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Sorry new category  object is empty!";
                response.SingleCategory= null;
                return response;
            }
        }

        public async Task<ServiceCategory> UpdateCategory(int IdCategory, string? name)
        {
            var response = new ServiceCategory();

            // Fetch the category
            var category = await GetCategory(IdCategory);
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
                    category.SingleCategory.Name = name;
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



        public async Task<ServiceCategory> DeleteCategory(int IdCategory)
        {
            var response = new ServiceCategory();
            var cate= await GetCategory(IdCategory);
            if (cate.SingleCategory != null)
            {
                appContext.Categories.Remove(cate.SingleCategory);
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
