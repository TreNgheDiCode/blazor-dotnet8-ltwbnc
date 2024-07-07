using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Interfaces
{
    public interface IOrderRepo
    {
        Task<ServiceModel<OrderList>> GetOrders(int? page, int? pageSize);
        Task<ServiceModel<OrderItem>> GetOrder(string id);
        Task<GeneralResponse> AddOrder(CreateOrderDTO order);
        Task<GeneralResponse> UpdateOrder(string id, UpdateOrderDTO order);
    }
}
