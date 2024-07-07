using BaseLibrary.DTOs;
using BaseLibrary.Helpers.Client;
using BaseLibrary.Responses;
using ClientAdminLibrary.Services.Contracts;
using System.Net.Http.Json;

namespace ClientAdminLibrary.Services.Implementations
{
    public class OrderService(GetHttpClient httpClient) : IOrderService
    {
        public const string OrderUrl = "api/orders";

        public Task<GeneralResponse> CreateOrder(CreateOrderDTO order)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> DeleteOrder(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceModel<OrderItem>> GetOrder(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceModel<OrderList>> GetOrders()
        {
            var client = await httpClient.GetPrivateHttpClient();

            var result = await client.GetFromJsonAsync<ServiceModel<OrderList>>(OrderUrl);

            if (result == null)
            {
                return new ServiceModel<OrderList>()
                {
                    Data = null,
                    Message = "Lỗi máy chủ",
                    Success = false
                };
            }

            return result;
        }

        public Task<GeneralResponse> UpdateOrder(int id, UpdateOrderDTO order)
        {
            throw new NotImplementedException();
        }
    }
}
