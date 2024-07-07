using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ClientAdminLibrary.Services.Contracts
{
    public interface IOrderService
    {
        Task<ServiceModel<OrderList>> GetOrders();
        Task<ServiceModel<OrderItem>> GetOrder(int id);
        Task<GeneralResponse> CreateOrder(CreateOrderDTO order);
        Task<GeneralResponse> UpdateOrder(int id, UpdateOrderDTO order);
        Task<GeneralResponse> DeleteOrder(int id);
    }
}
