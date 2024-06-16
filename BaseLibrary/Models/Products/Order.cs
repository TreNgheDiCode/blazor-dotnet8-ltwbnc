using BaseLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseLibrary.Models.Products
{
    public class Order
    {
        public string Id { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }

        // Quan hệ đến bảng Khách hàng: Một đơn hàng chỉ thuộc về một khách hàng
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        // Quan hệ đến bảng Sản phẩm: Một đơn hàng có thể chứa nhiều sản phẩm
        [JsonIgnore]
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
