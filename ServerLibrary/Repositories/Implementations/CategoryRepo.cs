using BaseLibrary.DTOs;
using BaseLibrary.Models.Products;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Repositories.Implementations
{
    public class CategoryRepo(AppDbContext context) : ICategoryRepo
    {
        public async Task<GeneralResponse> AddCategory(CreateCategoryDTO category)
        {
            // Kiểm tra xem danh mục đã tồn tại chưa
            bool isExist = await context.Categories.AnyAsync(c => c.Name == category.Name);

            // Nếu danh mục đã tồn tại
            if (isExist)
            {
                return new GeneralResponse(false, "Danh mục đã tồn tại");
            }

            // Tạo danh mục mới
            Category newCategory = new()
            {
                Name = category.Name
            };

            // Thêm danh mục mới vào cơ sở dữ liệu
            await context.Categories.AddAsync(newCategory);

            // Lưu thay đổi vào cơ sở dữ liệu
            await context.SaveChangesAsync();

            // Trả về thông báo thành công
            return new GeneralResponse(true, "Tạo danh mục thành công");
        }

        public async Task<GeneralResponse> DeleteCategory(int id)
        {
            // Lấy danh mục theo ID
            Category category = await context.Categories.FindAsync(id);

            // Nếu không tìm thấy danh mục
            if (category == null)
            {
                return new GeneralResponse(false, "Không tìm thấy danh mục");
            }

            // Nếu danh mục tồn tại sản phẩm, không thể xóa
            bool hasProduct = await context.Products.AnyAsync(p => p.CategoryId == id);

            if (hasProduct)
            {
                return new GeneralResponse(false, "Danh mục đang chứa sản phẩm, không thể xóa");
            }

            // Xóa danh mục
            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            // Trả về thông báo thành công
            return new GeneralResponse(true, "Xóa danh mục thành công");
        }

        public async Task<ServiceModel<CategoryList>> GetCategories(int? page, int? pageSize)
        {
            // Giá trị mặc định của page và pageSize
            int currentPage = page ?? 1; // Nếu page là null, trả về trang đầu tiên
            int currentPageSize = pageSize ?? await context.ApplicationUsers.CountAsync(); // Nếu pageSize là null, trả về tất cả

            // Tính tổng số lượng danh mục
            int totalCount = await context.Categories.CountAsync();

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            // Lấy danh sách danh mục
            List<CategoryItem> categories = await context.Categories
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .Select(c => new CategoryItem
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            // Nếu không có danh mục nào
            if (categories.Count == 0)
            {
                return new ServiceModel<CategoryList>
                {
                    Success = false,
                    Message = "Danh sách danh mục trống",
                    Data = null
                };
            }

            // Trả về danh sách danh mục
            return new ServiceModel<CategoryList>
            {
                Success = true,
                Message = "Lấy danh sách danh mục thành công",
                Data = new CategoryList
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = currentPage,
                    PageSize = currentPageSize,
                    Categories = categories
                }
            };
        }

        public async Task<ServiceModel<CategoryItem>> GetCategory(int id)
        {
            // Lấy danh mục theo ID
            CategoryItem category = await context.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryItem
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .FirstOrDefaultAsync();

            // Nếu không tìm thấy danh mục
            if (category == null)
            {
                return new ServiceModel<CategoryItem>
                {
                    Success = false,
                    Message = "Không tìm thấy danh mục",
                    Data = null
                };
            }

            // Trả về danh mục
            return new ServiceModel<CategoryItem>
            {
                Success = true,
                Message = "Lấy danh mục thành công",
                Data = category
            };
        }

        public async Task<GeneralResponse> UpdateCategory(int id, UpdateCategoryDTO category)
        {
            // Lấy danh mục theo ID
            Category updateCategory = await context.Categories.FindAsync(id);

            // Nếu không tìm thấy danh mục
            if (updateCategory == null)
            {
                return new GeneralResponse(false, "Không tìm thấy danh mục");
            }

            // Kiểm tra xem danh mục đã tồn tại chưa
            bool isExist = await context.Categories.AnyAsync(c => c.Name == category.Name);

            if (isExist)
            {
                return new GeneralResponse(false, "Danh mục đã tồn tại");
            }

            // Cập nhật thông tin danh mục
            updateCategory.Name = category.Name;

            // Lưu thay đổi vào cơ sở dữ liệu
            await context.SaveChangesAsync();

            // Trả về thông báo thành công
            return new GeneralResponse(true, "Cập nhật danh mục thành công");
        }
    }
}
