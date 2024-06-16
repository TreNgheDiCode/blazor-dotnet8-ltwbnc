using BaseLibrary.DTOs;
using BaseLibrary.Models.Products;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;

namespace ServerLibrary.Repositories.Implementations
{
    public class ProductRepo(AppDbContext context) : IProductRepo
    {
        public async Task<GeneralResponse> AddProduct(CreateProductDTO product)
        {
            // Kiểm tra danh sách danh mục trong cơ sở dữ liệu không rỗng
            if (!await context.Categories.AnyAsync())
            {
                return new GeneralResponse(false, "Không có danh mục nào trong cơ sở dữ liệu");
            }

            // Kiểm tra danh mục có tồn tại không
            bool isCategoryExist = await context.Categories.AnyAsync(c => c.Id == product.CategoryId);

            if (!isCategoryExist)
            {
                return new GeneralResponse(false, "Danh mục không tồn tại");
            }
            // Kiểm tra xem sản phẩm đã tồn tại với danh mục đó chưa
            bool isExist = await context.Products.AnyAsync(p => p.Name == product.Name && p.CategoryId == product.CategoryId);

            // Nếu sản phẩm đã tồn tại
            if (isExist)
            {
                return new GeneralResponse(false, "Sản phẩm với tên này đã tồn tại trong cùng danh mục");
            }

            // Tạo sản phẩm mới
            Product newProduct = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Discount = product.Discount,
                IsFlashSale = product.IsFlashSale,
                Status = product.Status,
                CategoryId = product.CategoryId,
                ProductImages = product.ProductImages.Select(pi => new ProductImage
                {
                    Url = pi.ImageUrl
                }).ToList(),
                ProductOptions = product.ProductOptions.Select(po => new ProductOption
                {
                    Color = po.Color,
                    Size = po.Size,
                    Quantity = po.Quantity
                }).ToList()
            };

            // Thêm sản phẩm mới vào cơ sở dữ liệu
            await context.Products.AddAsync(newProduct);

            // Lưu thay đổi vào cơ sở dữ liệu
            await context.SaveChangesAsync();

            // Trả về thông báo thành công
            return new GeneralResponse(true, "Tạo sản phẩm thành công");
        }

        public async Task<GeneralResponse> AddProductImages(int id, List<ProductImageDTO> productImages)
        {
            // Kiểm tra danh sách danh mục trong cơ sở dữ liệu không rỗng
            if (!await context.Categories.AnyAsync())
            {
                return new GeneralResponse(false, "Không có danh mục nào trong cơ sở dữ liệu");
            }

            // Lấy sản phẩm theo ID
            Product? product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return new GeneralResponse(false, "Không tìm thấy sản phẩm");
            }

            // Thêm hình ảnh vào sản phẩm
            product.ProductImages.AddRange(productImages.Select(pi => new ProductImage
            {
                Url = pi.ImageUrl
            }));

            // Lưu thay đổi vào cơ sở dữ liệu
            await context.SaveChangesAsync();

            return new GeneralResponse(true, "Thêm hình ảnh vào sản phẩm thành công");
        }

        public async Task<GeneralResponse> AddProductOptions(int id, List<ProductOptionDTO> productOptions)
        {
            // Kiểm tra danh sách danh mục trong cơ sở dữ liệu không rỗng
            if (!await context.Categories.AnyAsync())
            {
                return new GeneralResponse(false, "Không có danh mục nào trong cơ sở dữ liệu");
            }

            // Lấy sản phẩm theo ID
            Product? product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return new GeneralResponse(false, "Không tìm thấy sản phẩm");
            }

            // Thêm tùy chọn vào sản phẩm
            product.ProductOptions.AddRange(productOptions.Select(po => new ProductOption
            {
                Color = po.Color,
                Size = po.Size,
                Quantity = po.Quantity
            }));

            // Lưu thay đổi vào cơ sở dữ liệu
            await context.SaveChangesAsync();

            return new GeneralResponse(true, "Thêm tùy chọn vào sản phẩm thành công");
        }

        public async Task<GeneralResponse> DeleteProduct(int id)
        {
            // Kiểm tra danh sách danh mục trong cơ sở dữ liệu không rỗng
            if (!await context.Categories.AnyAsync())
            {
                return new GeneralResponse(false, "Không có danh mục nào trong cơ sở dữ liệu");
            }

            // Lấy sản phẩm theo ID
            Product? product = await context.Products.FindAsync(id);

            if (product == null)
            {
                return new GeneralResponse(false, "Không tìm thấy sản phẩm");
            }

            // Xóa sản phẩm
            context.Products.Remove(product);
            await context.SaveChangesAsync();

            return new GeneralResponse(true, "Xóa sản phẩm thành công");
        }

        public async Task<GeneralResponse> DeleteProductImage(int productId, int imageId)
        {
            // Kiểm tra danh sách danh mục trong cơ sở dữ liệu không rỗng
            if (!await context.Categories.AnyAsync())
            {
                return new GeneralResponse(false, "Không có danh mục nào trong cơ sở dữ liệu");
            }

            // Lấy sản phẩm theo ID
            Product? product = await context.Products.FindAsync(productId);
            if (product == null)
            {
                return new GeneralResponse(false, "Không tìm thấy sản phẩm");
            }

            // Lấy hình ảnh theo ID và thuộc sản phẩm đó
            ProductImage? productImage = await context.ProductImages.FirstOrDefaultAsync(pi => pi.Id == imageId && pi.ProductId == productId);
            if (productImage == null)
            {
                return new GeneralResponse(false, "Không tìm thấy hình ảnh");
            }

            // Xóa hình ảnh
            product.ProductImages.Remove(productImage);
            await context.SaveChangesAsync();

            return new GeneralResponse(true, "Xóa hình ảnh thành công");
        }

        public async Task<GeneralResponse> DeleteProductOption(int productId, int optionId)
        {
            // Kiểm tra danh sách danh mục trong cơ sở dữ liệu không rỗng
            if (!await context.Categories.AnyAsync())
            {
                return new GeneralResponse(false, "Không có danh mục nào trong cơ sở dữ liệu");
            }

            // Lấy sản phẩm theo ID
            Product? product = await context.Products.FindAsync(productId);
            if (product == null)
            {
                return new GeneralResponse(false, "Không tìm thấy sản phẩm");
            }

            // Lấy tùy chọn theo ID và thuộc sản phẩm đó
            ProductOption? productOption = await context.ProductOptions.FirstOrDefaultAsync(po => po.Id == optionId && po.ProductId == productId);

            if (productOption == null)
            {
                return new GeneralResponse(false, "Không tìm thấy tùy chọn");
            }

            // Xóa tùy chọn
            product.ProductOptions.Remove(productOption);
            await context.SaveChangesAsync();

            return new GeneralResponse(true, "Xóa tùy chọn thành công");
        }

        public async Task<ServiceModel<ProductItem>> GetProductById(int id)
        {
            // Kiểm tra danh sách danh mục trong cơ sở dữ liệu không rỗng
            if (!await context.Categories.AnyAsync())
            {
                return new ServiceModel<ProductItem>
                {
                    Success = false,
                    Message = "Không có danh mục nào trong cơ sở dữ liệu",
                    Data = null
                };
            }

            // Lấy sản phẩm theo ID, kèm danh mục của sản phẩm đó và danh sách hình ảnh của sản phẩm và danh sách tùy chọn sản phẩm và danh sách đánh giá của sản phẩm
            ProductItem? product = await context.Products
                .Select(p => new ProductItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Discount = p.Discount,
                    IsFlashSale = p.IsFlashSale,
                    Status = p.Status,
                    Category = new CategoryItem
                    {
                        Id = p.Category!.Id,
                        Name = p.Category.Name
                    },
                    ProductImages = p.ProductImages.Select(pi => new ProductItemImage
                    {
                        Id = pi.Id,
                        ImageUrl = pi.Url
                    }).ToList(),
                    ProductOptions = p.ProductOptions.Select(po => new ProductItemOption
                    {
                        Id = po.Id,
                        Color = po.Color,
                        Size = po.Size,
                        Quantity = po.Quantity
                    }).ToList(),
                    ProductReviews = p.ProductReviews.Select(pr => new ProductItemReview
                    {
                        Id = pr.Id,
                        Content = pr.Content,
                        Rating = pr.Rating,
                        CreatedAt = pr.CreatedAt,
                        UpdatedAt = pr.UpdatedAt,
                        User = new ProductItemReviewUserDetail
                        {
                            Id = pr.User!.Id,
                            FullName = pr.User.Fullname,
                            Photo = pr.User.Photo
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            // Nếu không tìm thấy sản phẩm
            if (product == null)
            {
                return new ServiceModel<ProductItem>
                {
                    Success = false,
                    Message = "Không tìm thấy sản phẩm",
                    Data = null
                };
            }

            // Trả về sản phẩm
            return new ServiceModel<ProductItem>
            {
                Success = true,
                Message = "Lấy sản phẩm thành công",
                Data = product
            };
        }

        public async Task<ServiceModel<ProductList>> GetProducts(int? page, int? pageSize)
        {
            // Kiểm tra danh sách danh mục trong cơ sở dữ liệu không rỗng
            if (!await context.Categories.AnyAsync())
            {
                return new ServiceModel<ProductList>
                {
                    Success = false,
                    Message = "Không có danh mục nào trong cơ sở dữ liệu",
                    Data = null
                };
            }

            // Giá trị mặc định của page và pageSize
            int currentPage = page ?? 1; // Nếu page là null, trả về trang đầu tiên
            int currentPageSize = pageSize ?? await context.Products.CountAsync(); // Nếu pageSize là null, trả về tất cả

            // Tính tổng số lượng danh mục
            int totalCount = await context.Products.CountAsync();

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            // Lấy danh sách sản phẩm, mỗi sản phẩm kèm danh mục của sản phẩm đó và danh sách hình ảnh của sản phẩm và danh sách tùy chọn sản phẩm và danh sách đánh giá của sản phẩm
            List<ProductItem> products = await context.Products
                .Select(p => new ProductItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Discount = p.Discount,
                    IsFlashSale = p.IsFlashSale,
                    Status = p.Status,
                    Category = new CategoryItem
                    {
                        Id = p.Category!.Id,
                        Name = p.Category.Name
                    },
                    ProductImages = p.ProductImages.Select(pi => new ProductItemImage
                    {
                        Id = pi.Id,
                        ImageUrl = pi.Url
                    }).ToList(),
                    ProductOptions = p.ProductOptions.Select(po => new ProductItemOption
                    {
                        Id = po.Id,
                        Color = po.Color,
                        Size = po.Size,
                        Quantity = po.Quantity
                    }).ToList(),
                    ProductReviews = p.ProductReviews.Select(pr => new ProductItemReview
                    {
                        Id = pr.Id,
                        Content = pr.Content,
                        Rating = pr.Rating,
                        CreatedAt = pr.CreatedAt,
                        UpdatedAt = pr.UpdatedAt,
                        User = new ProductItemReviewUserDetail
                        {
                            Id = pr.User!.Id,
                            FullName = pr.User.Fullname,
                            Photo = pr.User.Photo
                        }
                    }).ToList()
                })
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToListAsync();

            // Nếu không có sản phẩm nào
            if (products.Count == 0)
            {
                return new ServiceModel<ProductList>
                {
                    Success = false,
                    Message = "Danh sách sản phẩm trống",
                    Data = null
                };
            }

            // Trả về danh sách sản phẩm
            return new ServiceModel<ProductList>
            {
                Success = true,
                Message = "Lấy danh sách sản phẩm thành công",
                Data = new ProductList
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = currentPage,
                    PageSize = currentPageSize,
                    Products = products
                }
            };
        }

        public async Task<GeneralResponse> UpdateProduct(int id, UpdateProductDTO product)
        {
            // Kiểm tra danh sách danh mục trong cơ sở dữ liệu không rỗng
            if (!await context.Categories.AnyAsync())
            {
                return new GeneralResponse(false, "Không có danh mục nào trong cơ sở dữ liệu");
            }

            // Lấy sản phẩm theo ID
            Product? productToUpdate = await context.Products.FindAsync(id);

            // Nếu không tìm thấy sản phẩm
            if (productToUpdate == null)
            {
                return new GeneralResponse(false, "Không tìm thấy sản phẩm");
            }

            // Kiểm tra danh mục có tồn tại không
            bool isCategoryExist = await context.Categories.AnyAsync(c => c.Id == product.CategoryId);

            // Nếu danh mục không tồn tại
            if (!isCategoryExist)
            {
                return new GeneralResponse(false, "Danh mục không tồn tại");
            }

            // Kiểm tra xem sản phẩm đã tồn tại với danh mục đó chưa
            bool isExist = await context.Products.AnyAsync(p => p.Name == product.Name && p.CategoryId == product.CategoryId);

            // Nếu sản phẩm đã tồn tại với danh mục đó
            if (isExist)
            {
                return new GeneralResponse(false, "Sản phẩm với tên này đã tồn tại trong cùng danh mục");
            }

            // Cập nhật thông tin sản phẩm
            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Price = product.Price;
            productToUpdate.Discount = product.Discount;
            productToUpdate.IsFlashSale = product.IsFlashSale;
            productToUpdate.Status = product.Status;
            productToUpdate.CategoryId = product.CategoryId;
            productToUpdate.ProductImages = product.ProductImages.Select(pi => new ProductImage
            {
                Url = pi.ImageUrl
            }).ToList();
            productToUpdate.ProductOptions = product.ProductOptions.Select(po => new ProductOption
            {
                Color = po.Color,
                Size = po.Size,
                Quantity = po.Quantity
            }).ToList();

            // Lưu thay đổi vào cơ sở dữ liệu
            await context.SaveChangesAsync();

            // Trả về thông báo thành công
            return new GeneralResponse(true, "Cập nhật sản phẩm thành công");
        }

        public async Task<GeneralResponse> UpdateProductImage(int productId, int imageId, string imageUrl)
        {
            // Kiểm tra danh sách danh mục trong cơ sở dữ liệu không rỗng
            if (!await context.Categories.AnyAsync())
            {
                return new GeneralResponse(false, "Không có danh mục nào trong cơ sở dữ liệu");
            }

            // Lấy sản phẩm theo ID
            Product? product = await context.Products.FindAsync(productId);
            if (product == null)
            {
                return new GeneralResponse(false, "Không tìm thấy sản phẩm");
            }

            // Lấy hình ảnh theo ID và thuộc sản phẩm đó
            ProductImage? productImage = await context.ProductImages.FirstOrDefaultAsync(pi => pi.Id == imageId && pi.ProductId == productId);
            if (productImage == null)
            {
                return new GeneralResponse(false, "Không tìm thấy hình ảnh");
            }

            // Cập nhật đường dẫn hình ảnh
            productImage.Url = imageUrl;

            // Lưu thay đổi vào cơ sở dữ liệu
            await context.SaveChangesAsync();

            return new GeneralResponse(true, "Cập nhật hình ảnh thành công");
        }

        public async Task<GeneralResponse> UpdateProductOption(int productId, int optionId, ProductOptionDTO productOption)
        {
            // Lấy sản phẩm theo ID
            Product? product = await context.Products.FindAsync(productId);
            if (product == null)
            {
                return new GeneralResponse(false, "Không tìm thấy sản phẩm");
            }

            // Lấy tùy chọn theo ID và thuộc sản phẩm đó
            ProductOption? productOptionToUpdate = await context.ProductOptions.FirstOrDefaultAsync(po => po.Id == optionId && po.ProductId == productId);
            if (productOptionToUpdate == null)
            {
                return new GeneralResponse(false, "Không tìm thấy tùy chọn");
            }

            // Cập nhật thông tin tùy chọn
            productOptionToUpdate.Color = productOption.Color;
            productOptionToUpdate.Size = productOption.Size;
            productOptionToUpdate.Quantity = productOption.Quantity;

            // Lưu thay đổi vào cơ sở dữ liệu
            await context.SaveChangesAsync();

            return new GeneralResponse(true, "Cập nhật tùy chọn thành công");
        }
    }
}
