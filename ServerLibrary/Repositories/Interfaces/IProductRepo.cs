using BaseLibrary.DTOs;
using BaseLibrary.Enums;
using BaseLibrary.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IProductRepo
    {
        Task<ServiceModel<ProductsDTO>> AddProduct(ProductsDTO product);
        Task<ServiceModel<ProductsDTO>> GetProductById(int id);
        Task<ServiceModel<ProductsDTO>> GetProducts();
        //Task<ServiceModel<ProductsDTO>> UpdateProduct(int id,ProductsDTO product);
        Task<ServiceModel<ProductsDTO>> DeleteProduct(int id);
    }
}
