using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUserLibrary.Services.Contracts
{
    public interface IProductsService
    {
        Task<ServiceModel<ProductList>> GetProducts();
        Task<ServiceModel<ProductItem>> GetProduct(int id);
    }
}
