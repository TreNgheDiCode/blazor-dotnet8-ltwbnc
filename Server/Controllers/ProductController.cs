using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/products")]
    [ApiController]
    [AllowAnonymous]
    public class ProductController(IProductRepo productRepo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts(int? page, int? pageSize)
        {
            var result = await productRepo.GetProducts(page, pageSize);

            if (result.Success == true)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await productRepo.GetProductById(id);

            if (result.Success == true)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(CreateProductDTO product)
        {
            var result = await productRepo.AddProduct(product);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("{id}/images")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProductImages(int id, List<ProductImageDTO> productImages)
        {
            var result = await productRepo.AddProductImages(id, productImages);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("{id}/options")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProductOptions(int id, List<ProductOptionDTO> productOptions)
        {
            var result = await productRepo.AddProductOptions(id, productOptions);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDTO product)
        {
            var result = await productRepo.UpdateProduct(id, product);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("{productId}/images/{imageId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductImage(int productId, int imageId, string imageUrl)
        {
            var result = await productRepo.UpdateProductImage(productId, imageId, imageUrl);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("{productId}/options/{optionId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductOption(int productId, int optionId, ProductOptionDTO productOption)
        {
            var result = await productRepo.UpdateProductOption(productId, optionId, productOption);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await productRepo.DeleteProduct(id);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpDelete("{productId}/images/{imageId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductImage(int productId, int imageId)
        {
            var result = await productRepo.DeleteProductImage(productId, imageId);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpDelete("{productId}/options/{optionId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductOption(int productId, int optionId)
        {
            var result = await productRepo.DeleteProductOption(productId, optionId);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("{id}/flash-sale")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FlashSale(int id)
        {
            var result = await productRepo.FlashSale(id);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
    
