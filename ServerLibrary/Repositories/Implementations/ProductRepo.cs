using BaseLibrary.DTOs;
using BaseLibrary.Enums;
using BaseLibrary.Models.Products;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Repositories.Implementations
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext appContext;
        public ProductRepo(AppDbContext appDbContext)
        {
            appContext = appDbContext;
        }

        public async Task<ServiceModel<ProductsDTO>> AddProduct(ProductsDTO product)
        {
            var response = new ServiceModel<ProductsDTO>();

            if (product == null)
            {
                response.Success = false;
                response.Message = "Product is null";
                return response;
            }
            else
            {
                try
                {
                    var newProduct = new Product
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Discount = product.Discount,
                        IsFlashSale = product.IsFlashSale,
                        Status = product.Status,
                        CategoryId = product.CategoryId
                    };

                    await appContext.Products.AddAsync(newProduct);
                    await appContext.SaveChangesAsync();

                    response.Single = product;
                    response.Message = "Product added successfully";
                    return response;
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi chi tiết
                    response.Success = false;
                    response.Message = ex.Message;
                    return response;
                }
            }


        }

        //public async Task<ServiceModel<ProductsDTO>> UpdateProduct(int id ,ProductsDTO product)
        //{
        //    var response = new ServiceModel<ProductsDTO>();

        //    try
        //    {
        //        // Truy vấn cơ sở dữ liệu để lấy thông tin của sản phẩm cần cập nhật
        //        var existingProduct = await appContext.Products.FindAsync(id);

        //        if (existingProduct == null)
        //        {
        //            // Nếu sản phẩm không tồn tại, trả về một ServiceModel với thông báo lỗi
        //            response.Success = false;
        //            response.Message = "Product not found";
        //            return response;
        //        }

        //        // Cập nhật thông tin của sản phẩm với các giá trị mới từ product
        //        existingProduct.Name = product.Name;
        //        existingProduct.Description = product.Description;
        //        existingProduct.Price = product.Price;
        //        existingProduct.Discount = product.Discount;
        //        existingProduct.IsFlashSale = product.IsFlashSale;
        //        existingProduct.Status = product.Status;
        //        existingProduct.CategoryId = product.CategoryId;

        //        // Lưu các thay đổi vào cơ sở dữ liệu
        //        await appContext.SaveChangesAsync();

        //        // Cập nhật dữ liệu trả về với thông tin sản phẩm đã được cập nhật
        //        var updatedProductDTO = new ProductsDTO
        //        {
        //            Id = existingProduct.Id,
        //            Name = existingProduct.Name,
        //            Description = existingProduct.Description,
        //            Price = existingProduct.Price,
        //            Discount = existingProduct.Discount,
        //            IsFlashSale = existingProduct.IsFlashSale,
        //            Status = existingProduct.Status,
        //            CategoryId = existingProduct.CategoryId
        //        };

        //        response.Single = updatedProductDTO;
        //        response.Message = "Product updated successfully";
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Ghi log lỗi chi tiết
        //        response.Success = false;
        //        response.Message = ex.Message;
        //        return response;
        //    }
        //}


        public async Task<ServiceModel<ProductsDTO>> DeleteProduct(int id)
        {
            var response = new ServiceModel<ProductsDTO>();

            try
            {
                // Truy vấn cơ sở dữ liệu để lấy thông tin của sản phẩm cần xóa
                var productToDelete = await appContext.Products.FindAsync(id);
                if (productToDelete == null)
                {
                    // Nếu sản phẩm không tồn tại, trả về một ServiceModel với thông báo lỗi
                    response.Success = false;
                    response.Message = "Product not found";
                    return response;
                }
                // Xóa sản phẩm khỏi cơ sở dữ liệu
                appContext.Products.Remove(productToDelete);



                // Lưu các thay đổi vào cơ sở dữ liệu
                await appContext.SaveChangesAsync();

                response.Message = "Product deleted successfully";
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi chi tiết
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }




        public async Task<ServiceModel<ProductsDTO>> GetProductById(int id)
        {
            var response = new ServiceModel<ProductsDTO>();

            try
            {
                // Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm theo Id
                var product = await appContext.Products.FindAsync(id);

                if (product == null)
                {
                    response.Success = false;
                    response.Message = "Product not found";
                    return response;
                }

                // Chuyển đổi thông tin sản phẩm sang ProductsDTO
                var productDTO = new ProductsDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Discount = product.Discount,
                    IsFlashSale = product.IsFlashSale,
                    Status = product.Status,
                    CategoryId = product.CategoryId
                };

                response.Single = productDTO;
                response.Message = "Product retrieved successfully";
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi chi tiết
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }


        public async Task<ServiceModel<ProductsDTO>> GetProducts()
        {
            var response = new ServiceModel<ProductsDTO>();

            try
            {
                // Truy vấn cơ sở dữ liệu để lấy danh sách sản phẩm
                var products = await appContext.Products.ToListAsync();

                // Chuyển đổi danh sách sản phẩm sang danh sách ProductsDTO
                var productDTOs = products.Select(product => new ProductsDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Discount = product.Discount,
                    IsFlashSale = product.IsFlashSale,
                    Status = product.Status,
                    CategoryId = product.CategoryId
                }).ToList();

                response.List = productDTOs;
                response.Message = "Products retrieved successfully";
                return response;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi chi tiết
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }


    }
}
