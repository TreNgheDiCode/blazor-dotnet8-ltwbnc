using BaseLibrary.DTOs;
using BaseLibrary.Models.Products;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Repositories.Implementations
{
    public class DiscountRepo(AppDbContext context) : IDiscountRepo
    {
        public async Task<GeneralResponse> AddDiscount(CreateDiscountDTO discount)
        {
            // Nếu thêm giảm giá mà chọn sản phẩm, kiểm tra xem sản phẩm có tồn tại không
            if (discount.ProductId != null)
            {
                bool isProductExist = await context.Products.AnyAsync(p => p.Id == discount.ProductId);

                if (!isProductExist)
                {
                    return new GeneralResponse(false, "Sản phẩm không tồn tại");
                }
            }

            // Kiểm tra xem giảm giá đã tồn tại cho sản phẩm với % giảm giá đó chưa
            bool isExist = await context.Discounts.AnyAsync(d => d.ProductId == discount.ProductId && d.Value == discount.Value);

            // Nếu giảm giá đã tồn tại
            if (isExist)
            {
                return new GeneralResponse(false, "Giảm giá đã tồn tại cho sản phẩm với % giảm giá đó");
            }

            // Nếu ngày bắt đầu mới lớn hơn ngày kết thúc mới
            if (discount.StartDate > discount.EndDate)
            {
                return new GeneralResponse(false, "Ngày bắt đầu không thể lớn hơn ngày kết thúc");
            }

            // Kiểm tra ngày bắt đầu mới có hợp lệ không
            if (discount.StartDate < DateTime.Now)
            {
                return new GeneralResponse(false, "Ngày bắt đầu không hợp lệ");
            }

            // Kiểm tra ngày kết thúc mới có hợp lệ không
            if (discount.EndDate < DateTime.Now)
            {
                return new GeneralResponse(false, "Ngày kết thúc không hợp lệ");
            }

            // Nếu số lượng mã giảm giá nhiều hơn tổng số lượng người thực tế trong cơ sở dữ liệu, không thể thêm mới
            if (discount.Quantity > await context.ApplicationUsers.CountAsync())
            {
                return new GeneralResponse(false, "Số lượng mã giảm giá nhiều hơn tổng số lượng người thực tế trong cơ sở dữ liệu, không thể thêm mới");
            }

            // Tạo giảm giá mới
            Discount newDiscount = new()
            {
                Name = discount.Name,
                Value = discount.Value,
                Quantity = discount.Quantity,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                ProductId = discount.ProductId
            };

            // Thêm giảm giá mới vào cơ sở dữ liệu
            await context.Discounts.AddAsync(newDiscount);

            // Lưu thay đổi vào cơ sở dữ liệu
            await context.SaveChangesAsync();

            // Trả về thông báo thành công
            return new GeneralResponse(true, "Tạo giảm giá thành công");
        }

        public async Task<GeneralResponse> DeleteDiscount(int id)
        {
            // Lấy giảm giá theo ID
            Discount? discount = await context.Discounts.FindAsync(id);

            // Nếu không tìm thấy giảm giá
            if (discount == null)
            {
                return new GeneralResponse(false, "Không tìm thấy giảm giá");
            }

            // Nếu giảm giá có người sở hữu và ngày kết thúc chưa đến, không thể xóa
            bool hasOwner = await context.DiscountWarehouses.AnyAsync(o => o.DiscountId == id && o.IsUsed == false);
            bool isNotExpired = discount.EndDate > DateTime.Now;

            if (hasOwner && isNotExpired)
            {
                return new GeneralResponse(false, "Giảm giá đang có người sở hữu và chưa hết hạn, không thể xóa");
            }

            // Xóa giảm giá
            context.Discounts.Remove(discount);
            await context.SaveChangesAsync();

            // Trả về thông báo thành công
            return new GeneralResponse(true, "Xóa giảm giá thành công");
        }

        public async Task<ServiceModel<DiscountItem>> GetDiscount(int id)
        {
            // Lấy giảm giá theo ID, kèm thông tin sản phẩm và danh sách chủ sở hữu, sản phẩm có thể trống
            DiscountItem? discount = await context.Discounts
                .Where(d => d.Id == id)
                .Select(d => new DiscountItem
                {
                    Id = d.Id,
                    Name = d.Name,
                    Value = d.Value,
                    Quantity = d.Quantity,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    Product = d.Product == null ? null : new DiscountProductDTO
                    {
                        ProductId = d.Product.Id,
                        Name = d.Product.Name
                    },
                    Owners = d.Owners.Select(o => new DiscountWarehouseItemDTO
                    {
                        UserId = o.UserId,
                        Name = o.User!.Fullname,
                        IsUsed = o.IsUsed
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            // Nếu không tìm thấy giảm giá
            if (discount == null)
            {
                return new ServiceModel<DiscountItem>
                {
                    Success = false,
                    Message = "Không tìm thấy giảm giá",
                    Data = null
                };
            }

            // Trả về giảm giá
            return new ServiceModel<DiscountItem>
            {
                Success = true,
                Message = "Lấy giảm giá thành công",
                Data = discount
            };
        }

        public async Task<ServiceModel<DiscountList>> GetDiscounts(int? page, int? pageSize)
        {
            // Giá trị mặc định của page và pageSize
            int currentPage = page ?? 1; // Nếu page là null, trả về trang đầu tiên
            int currentPageSize = pageSize ?? await context.Discounts.CountAsync(); // Nếu pageSize là null, trả về tất cả

            // Tính tổng số lượng giảm giá
            int totalCount = await context.Discounts.CountAsync();

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            // Lấy danh sách giảm giá, mỗi giảm giá kèm thông tin sản phẩm và danh sách chủ sở hữu, sản phẩm có thể trống
            List<DiscountItem> discounts = await context.Discounts
                .Select(d => new DiscountItem
                {
                    Id = d.Id,
                    Name = d.Name,
                    Value = d.Value,
                    Quantity = d.Quantity,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    Product = d.Product == null ? null : new DiscountProductDTO
                    {
                        ProductId = d.Product.Id,
                        Name = d.Product.Name
                    },
                    Owners = d.Owners.Select(o => new DiscountWarehouseItemDTO
                    {
                        UserId = o.UserId,
                        Name = o.User!.Fullname,
                        IsUsed = o.IsUsed
                    }).ToList()
                })
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToListAsync();
           
            // Nếu không có giảm giá nào
            if (discounts.Count == 0)
            {
                return new ServiceModel<DiscountList>
                {
                    Success = false,
                    Message = "Danh sách giảm giá trống",
                    Data = null
                };
            }

            // Trả về danh sách giảm giá
            return new ServiceModel<DiscountList>
            {
                Success = true,
                Message = "Lấy danh sách giảm giá thành công",
                Data = new DiscountList
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = currentPage,
                    PageSize = currentPageSize,
                    Discounts = discounts
                }
            };
        }

        public async Task<GeneralResponse> UpdateDiscount(int id, UpdateDiscountDTO discount)
        {
            // Lấy giảm giá theo ID
            Discount? currentDiscount = await context.Discounts.FindAsync(id);
            if (currentDiscount == null)
            {
                return new GeneralResponse(false, "Không tìm thấy giảm giá");
            }

            // Nếu thay đổi giảm giá mà chọn sản phẩm, kiểm tra xem sản phẩm có tồn tại không
            if (discount.ProductId != null)
            {
                bool isProductExist = await context.Products.AnyAsync(p => p.Id == discount.ProductId);

                if (!isProductExist)
                {
                    return new GeneralResponse(false, "Sản phẩm không tồn tại");
                }
            }

            // Kiểm tra xem giảm giá đã tồn tại cho sản phẩm với % giảm giá đó chưa
            bool isExist = await context.Discounts.AnyAsync(d => d.ProductId == discount.ProductId && d.Value == discount.Value && d.Id != id);

            // Nếu giảm giá đã tồn tại
            if (isExist)
            {
                return new GeneralResponse(false, "Giảm giá đã tồn tại cho sản phẩm với % giảm giá đó");
            }

            // Kiểm tra nếu % giảm giá thay đổi so với giá trị ban đầu, nếu giảm giá có người sở hữu và ngày kết thúc chưa đến, không thể cập nhật % giảm giá
            bool hasOwner = await context.DiscountWarehouses.AnyAsync(o => o.DiscountId == id && o.IsUsed == false);
            bool isNotExpired = currentDiscount.EndDate > DateTime.Now;

            if (hasOwner && isNotExpired && currentDiscount.Value != discount.Value)
            {
                return new GeneralResponse(false, "Giảm giá đang có người sở hữu và chưa hết hạn, không thể cập nhật % giảm giá");
            }

            // Nếu số lượng mã giảm giá nhiều hơn tổng số lượng người thực tế trong cơ sở dữ liệu, không thể cập nhật số lượng
            if (discount.Quantity > await context.ApplicationUsers.CountAsync())
            {
                return new GeneralResponse(false, "Số lượng mã giảm giá nhiều hơn tổng số lượng người thực tế trong cơ sở dữ liệu, không thể cập nhật số lượng");
            }

            // Nếu ngày bắt đầu mới lớn hơn ngày kết thúc mới
            if (discount.StartDate > discount.EndDate)
            {
                return new GeneralResponse(false, "Ngày bắt đầu không thể lớn hơn ngày kết thúc");
            }

            // Kiểm tra ngày bắt đầu mới có hợp lệ không
            if (discount.StartDate < DateTime.Now)
            {
                return new GeneralResponse(false, "Ngày bắt đầu không hợp lệ");
            }

            // Kiểm tra ngày kết thúc mới có hợp lệ không
            if (discount.EndDate < DateTime.Now)
            {
                return new GeneralResponse(false, "Ngày kết thúc không hợp lệ");
            }

            // Cập nhật thông tin giảm giá
            currentDiscount.Name = discount.Name;
            currentDiscount.Value = discount.Value;
            currentDiscount.Quantity = discount.Quantity;
            currentDiscount.StartDate = discount.StartDate;
            currentDiscount.EndDate = discount.EndDate;
            currentDiscount.ProductId = discount.ProductId;

            // Lưu thay đổi vào cơ sở dữ liệu
            await context.SaveChangesAsync();

            // Trả về thông báo thành công
            return new GeneralResponse(true, "Cập nhật giảm giá thành công");
        }
    }
}
