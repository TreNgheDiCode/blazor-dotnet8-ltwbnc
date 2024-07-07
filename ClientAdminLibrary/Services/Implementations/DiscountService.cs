using BaseLibrary.DTOs;
using BaseLibrary.Helpers.Client;
using BaseLibrary.Responses;
using ClientAdminLibrary.Services.Contracts;
using System.Net.Http.Json;

namespace ClientAdminLibrary.Services.Implementations
{
    public class DiscountService(GetHttpClient httpClient) : IDiscountService
    {
        public const string DiscountUrl = "api/discounts";

        public Task<GeneralResponse> CreateDiscount(CreateDiscountDTO discount)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> DeleteDiscount(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceModel<DiscountItem>> GetDiscount(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceModel<DiscountList>> GetDiscounts()
        {
            var client = httpClient.GetPublicHttpClient();

            var result = await client.GetFromJsonAsync<ServiceModel<DiscountList>>(DiscountUrl);

            // Nếu không có kết quả thì trả về thông báo lỗi
            if (result == null)
            {
                return new ServiceModel<DiscountList>()
                {
                    Data = null,
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }

            return result;
        }

        public Task<GeneralResponse> UpdateDiscount(int id, UpdateDiscountDTO discount)
        {
            throw new NotImplementedException();
        }
    }
}
