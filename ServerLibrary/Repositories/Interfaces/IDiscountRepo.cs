using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IDiscountRepo
    {
       Task<ServiceModel<DiscountList>> GetDiscounts(int? page, int? pageSize);
        Task<ServiceModel<DiscountItem>> GetDiscount(int id);
        Task<GeneralResponse> AddDiscount(CreateDiscountDTO discount);
        Task<GeneralResponse> UpdateDiscount(int id, UpdateDiscountDTO discount);
        Task<GeneralResponse> DeleteDiscount(int id);
    }
}
