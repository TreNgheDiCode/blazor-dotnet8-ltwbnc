using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/products")]
    [ApiController]
    [AllowAnonymous]
    //[Authorize(Roles = "Admin")]
    public class ProductController(IProductRepo productRepo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts(int? page, int? pageSize)
        {
            var result = await productRepo.GetProducts(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await productRepo.GetProductById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(CreateProductDTO product)
        {
            var result = await productRepo.AddProduct(product);
            return Ok(result);
        }

        [HttpPost]
        [Route("{id}/images")]
        public async Task<IActionResult> AddProductImages(int id, List<ProductImageDTO> productImages)
        {
            var result = await productRepo.AddProductImages(id, productImages);
            return Ok(result);
        }

        [HttpPost]
        [Route("{id}/options")]
        public async Task<IActionResult> AddProductOptions(int id, List<ProductOptionDTO> productOptions)
        {
            var result = await productRepo.AddProductOptions(id, productOptions);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDTO product)
        {
            var result = await productRepo.UpdateProduct(id, product);
            return Ok(result);
        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateProductImage(int productId, int imageId, string imageUrl)
        {
            var result = await productRepo.UpdateProductImage(productId, imageId, imageUrl);
            return Ok(result);
        }

        [HttpPut("{productId}/options/{optionId}")]
        public async Task<IActionResult> UpdateProductOption(int productId, int optionId, ProductOptionDTO productOption)
        {
            var result = await productRepo.UpdateProductOption(productId, optionId, productOption);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await productRepo.DeleteProduct(id);
            return Ok(result);
        }
    }
}
    
