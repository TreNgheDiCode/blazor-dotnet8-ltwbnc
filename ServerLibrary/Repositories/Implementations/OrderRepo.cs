using BaseLibrary.DTOs;
using BaseLibrary.Enums;
using BaseLibrary.Models.Products;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Repositories.Implementations
{
    public class OrderRepo(AppDbContext context) : IOrderRepo
    {
        public async Task<GeneralResponse> AddOrder(CreateOrderDTO order)
        {
            // Kiểm tra ngày tạo đơn hàng có hợp lệ không (ngày không được là ngày hôm qua)
            if (order.OrderDate.Date < DateTime.Now.Date)
            {
                return new GeneralResponse(false, "Ngày tạo đơn hàng không hợp lệ");
            }

            // Kiểm tra khách hàng có tồn tại không
            bool isExist = await context.ApplicationUsers.AnyAsync(u => u.Id == order.UserId);

            if (!isExist)
            {
                return new GeneralResponse(false, "Khách hàng không tồn tại");
            }

            // Kiểm tra sản phẩm có tồn tại không
            foreach (var item in order.OrderItemDetails)
            {
                bool isProductExist = await context.Products.AnyAsync(p => p.Id == item.ProductId);

                // Kiểm tra mã giảm giá có tồn tại không
                if (item.DiscountId != null)
                {
                    bool isDiscountExist = await context.Discounts.AnyAsync(d => d.Id == item.DiscountId);

                    if (!isDiscountExist)
                    {
                        return new GeneralResponse(false, $"Mã giảm giá {item.DiscountId} không tồn tại");
                    }

                    // Kiểm tra mã giảm giá còn hạn không
                    bool isDiscountValid = await context.Discounts.AnyAsync(d => d.Id == item.DiscountId && d.EndDate >= DateTime.Now);

                    if (!isDiscountValid)
                    {
                        return new GeneralResponse(false, $"Mã giảm giá {item.DiscountId} đã hết hạn");
                    }

                    // Kiểm tra mã giảm giá có áp dụng cho sản phẩm không
                    bool isDiscountApplyForProduct = await context.Discounts.AnyAsync(d => d.Id == item.DiscountId && d.ProductId == item.ProductId);

                    if (!isDiscountApplyForProduct)
                    {
                        return new GeneralResponse(false, $"Mã giảm giá {item.DiscountId} không áp dụng cho sản phẩm này");
                    }

                    // Kiểm tra mã giảm giá đã sử dụng hết chưa (quantity = 0)
                    bool isDiscountUsedUp = await context.Discounts.AnyAsync(d => d.Id == item.DiscountId && d.Quantity == 0);
                    if (!isDiscountUsedUp)
                    {
                        return new GeneralResponse(false, $"Mã giảm giá {item.DiscountId} đã hết lượt sử dụng");
                    }

                    // Kiểm tra mã giảm giá đã sử dụng chưa (dựa trên mã người dùng và mã giảm giá trong bảng DiscountWarehouse và IsUsed)
                    bool isDiscountUsed = await context.DiscountWarehouses.AnyAsync(dw => dw.UserId == order.UserId && dw.DiscountId == item.DiscountId && dw.IsUsed == true);

                    if (isDiscountUsed)
                    {
                        return new GeneralResponse(false, $"Mã giảm giá {item.DiscountId} đã được sử dụng");
                    }
                }

                // Trả về sản phẩm với mã không tồn tại
                if (!isProductExist)
                {
                    return new GeneralResponse(false, $"Sản phẩm với mã {item.ProductId} không tồn tại");
                }

                // Kiểm tra tùy chọn sản phẩm với mã sản phẩm và mã tùy chọn đó có tồn tại không
                bool isProductOptionExist = await context.ProductOptions.AnyAsync(po => po.ProductId == item.ProductId && po.Id == item.ProductOptionId);

                // Trả về tùy chọn sản phẩm với mã không tồn tại
                if (!isProductOptionExist)
                {
                    return new GeneralResponse(false, $"Tùy chọn sản phẩm với mã {item.ProductOptionId} không tồn tại");
                }

                // Kiểm tra số lượng sản phẩm còn đủ không
                ProductOption productOption = await context.ProductOptions.FindAsync(item.ProductOptionId);

                if (productOption.Quantity < item.Quantity)
                {
                    return new GeneralResponse(false, $"Số lượng sản phẩm {productOption.Product.Name} {productOption.Size} {productOption.Color} không đủ");
                }
            }

            // Tạo đơn hàng mới
            Order newOrder = new()
            {
                PaymentMethod = order.PaymentMethod,
                OrderDate = order.OrderDate,
                UserId = order.UserId,
                Status = OrderStatus.Pending,
                OrderDetails = order.OrderItemDetails.Select(item => new OrderDetail
                {
                    ProductId = item.ProductId,
                    ProductOptionId = item.ProductOptionId,
                    Quantity = item.Quantity,
                    DiscountId = item.DiscountId
                }).ToList()
            };

            // Giảm so lượng mã giảm giá đi 1
            foreach (var item in order.OrderItemDetails)
            {
                if (item.DiscountId != null)
                {
                    Discount discount = await context.Discounts.FindAsync(item.DiscountId);
                    discount.Quantity--;

                    // Cập nhật thay đổi vào cơ sở dữ liệu
                    context.Discounts.Update(discount);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await context.SaveChangesAsync();
                }
            }

            // Thêm đơn hàng mới vào cơ sở dữ liệu
            await context.Orders.AddAsync(newOrder);

            // Lưu thay đổi vào cơ sở dữ liệu
            await context.SaveChangesAsync();

            // Trả về thông báo thành công
            return new GeneralResponse(true, "Tạo đơn hàng thành công");
        }

        public async Task<ServiceModel<OrderItem>> GetOrder(string id)
        {
            // Lấy đơn hàng theo ID
            OrderItem? order = await context.Orders
                .Where(o => o.Id == id)
                .Select(o => new OrderItem
                {
                    Id = o.Id,
                    PaymentMethod = o.PaymentMethod,
                    Status = o.Status,
                    OrderDate = o.OrderDate,
                    User = new OrderUser
                    {
                        Fullname = o.User.Fullname,
                        Email = o.User.Email,
                        PhoneNumber = o.User.PhoneNumber,
                        Address = o.User.Address!.AddressDetail + ", " + o.User.Address.Ward!.FullName + ", " + o.User.Address.District!.FullName + ", " + o.User.Address.Province!.FullName
                    },
                    OrderItemDetails = o.OrderDetails.Select(od => new OrderItemDetail
                    {
                        Id = od.Id,
                        CoverUrl = od.Product.ProductImages.Select(pi => pi.Url).FirstOrDefault() ?? string.Empty,
                        Name = od.Product.Name,
                        Size = od.ProductOption.Size,
                        Color = od.ProductOption.Color,
                        Price = od.Product.Price,
                        Quantity = od.Quantity,
                        FlashSale = od.Product.Discount,
                        Discount = od.Discount.Value
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            // Nếu không tìm thấy đơn hàng
            if (order == null)
            {
                return new ServiceModel<OrderItem>
                {
                    Success = false,
                    Message = "Không tìm thấy đơn hàng",
                    Data = null
                };
            }

            // Trả về đơn hàng
            return new ServiceModel<OrderItem>
            {
                Success = true,
                Message = "Lấy đơn hàng thành công",
                Data = order
            };
        }

        public async Task<ServiceModel<OrderList>> GetOrders(int? page, int? pageSize)
        {
            // Giá trị mặc định của page và pageSize
            int currentPage = page ?? 1; // Nếu page = null thì mặc định là 1
            int currentPageSize = pageSize ?? await context.Orders.CountAsync(); // Nếu pageSize = null thì lấy tất cả

            // Tính tổng số lượng đơn hàng
            int totalCount = await context.Orders.CountAsync();

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            // Lấy danh sách đơn hàng
            List<OrderItem> orders = await context.Orders
                .Select(o => new OrderItem
                {
                    Id = o.Id,
                    PaymentMethod = o.PaymentMethod,
                    Status = o.Status,
                    OrderDate = o.OrderDate,
                    User = new OrderUser
                    {
                        Fullname = o.User.Fullname,
                        Email = o.User.Email,
                        PhoneNumber = o.User.PhoneNumber,
                        Address = o.User.Address!.AddressDetail + ", " + o.User.Address.Ward!.FullName + ", " + o.User.Address.District!.FullName + ", " + o.User.Address.Province!.FullName
                    },
                    OrderItemDetails = o.OrderDetails.Select(od => new OrderItemDetail
                    {
                        Id = od.Id,
                        CoverUrl = od.Product.ProductImages.Select(pi => pi.Url).FirstOrDefault() ?? string.Empty,
                        Name = od.Product.Name,
                        Size = od.ProductOption.Size,
                        Color = od.ProductOption.Color,
                        Price = od.Product.Price,
                        Quantity = od.Quantity,
                        FlashSale = od.Product.Discount,
                        Discount = od.Discount.Value
                    }).ToList()
                })
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToListAsync();

            // Nếu không có đơn hàng nào
            if (orders.Count == 0)
            {
                return new ServiceModel<OrderList>
                {
                    Success = false,
                    Message = "Danh sách đơn hàng trống",
                    Data = null
                };
            }

            // Trả về danh sách đơn hàng
            return new ServiceModel<OrderList>
            {
                Success = true,
                Message = "Lấy danh sách đơn hàng thành công",
                Data = new OrderList
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = currentPage,
                    PageSize = currentPageSize,
                    Orders = orders
                }
            };
        }

        public async Task<GeneralResponse> UpdateOrder(string id, UpdateOrderDTO order)
        {
            // Kiểm tra đơn hàng có tồn tại không
            Order? existingOrder = await context.Orders.FindAsync(id);

            if (existingOrder == null)
            {
                return new GeneralResponse(false, "Đơn hàng không tồn tại");
            }

            // Kiểm tra trạng thái đơn hàng có hợp lệ không
            if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Completed && order.Status != OrderStatus.Shipping && order.Status != OrderStatus.Processing && order.Status != OrderStatus.Cancelled)
            {
                return new GeneralResponse(false, "Trạng thái đơn hàng không hợp lệ");
            }

            // Kiểm tra ngày cập nhật đơn hàng có hợp lệ không (ngày không được là ngày hôm qua)
            if (order.OrderDate.Date < DateTime.Now.Date)
            {
                return new GeneralResponse(false, "Ngày cập nhật đơn hàng không hợp lệ");
            }

            // Cập nhật thông tin đơn hàng
            existingOrder.PaymentMethod = order.PaymentMethod;
            existingOrder.Status = order.Status;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.Status = order.Status;

            // Cập nhật thay đổi vào cơ sở dữ liệu
            context.Orders.Update(existingOrder);

            // Lưu thay đổi vào cơ sở dữ liệu
            await context.SaveChangesAsync();

            // Trả về thông báo thành công
            return new GeneralResponse(true, "Cập nhật đơn hàng thành công");
        }
    }
}
