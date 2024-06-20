using BaseLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUserLibrary.Services.Contracts
{
    public interface ICategoryService
    {
        public Task<ServiceModel<CategoryList>> GetCategories(int? page, int? pageSize);

    }
}
