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

        public async Task<GeneralResponse> CreateDiscount(CreateDiscountDTO discount)
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.PostAsJsonAsync(DiscountUrl, discount);

            // Nếu không có kết quả thì trả về thông báo lỗi
            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return successResponse ?? new GeneralResponse(false, "Thêm giảm giá thành công");
            }

            var errorResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

            return errorResponse ?? new GeneralResponse(false, "Thêm giảm giá thất bại");
        }

        public async Task<GeneralResponse> DeleteDiscount(int id)
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.DeleteAsync($"{DiscountUrl}/{id}");

            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return successResponse ?? new GeneralResponse(false, "Xóa giảm giá thành công");
            }

            var errorResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

            return errorResponse ?? new GeneralResponse(false, "Xóa giảm giá thất bại");
        }

        public async Task<ServiceModel<DiscountItem>> GetDiscount(int id)
        {
            var client = httpClient.GetPublicHttpClient();

            var result = await client.GetFromJsonAsync<ServiceModel<DiscountItem>>($"{DiscountUrl}/{id}");

            // Nếu không có kết quả thì trả về thông báo lỗi
            if (result == null)
            {
                return new ServiceModel<DiscountItem>()
                {
                    Data = null,
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }

            return result;
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

        public async Task<GeneralResponse> UpdateDiscount(int id, UpdateDiscountDTO discount)
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.PutAsJsonAsync($"{DiscountUrl}/{id}", discount);

            if (result.IsSuccessStatusCode)
            {
                var successResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

                return successResponse ?? new GeneralResponse(false, "Cập nhật giảm giá thành công");
            }

            var errorResponse = await result.Content.ReadFromJsonAsync<GeneralResponse>();

            return errorResponse ?? new GeneralResponse(false, "Cập nhật giảm giá thất bại");
        }
    }
}
