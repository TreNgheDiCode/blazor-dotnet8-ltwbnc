using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ClientAdminLibrary.Services.Contracts
{
    public interface IDiscountService
    {
        Task<ServiceModel<DiscountList>> GetDiscounts();
        Task<ServiceModel<DiscountItem>> GetDiscount(int id);
        Task<GeneralResponse> CreateDiscount(CreateDiscountDTO discount);
        Task<GeneralResponse> UpdateDiscount(int id, UpdateDiscountDTO discount);
        Task<GeneralResponse> DeleteDiscount(int id);
    }
}
