using BaseLibrary.DTOs;
using BaseLibrary.Enums;
using BaseLibrary.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo productRepo;
        public ProductController(IProductRepo productRepo)
        {
            this.productRepo = productRepo;
        }

        [HttpGet("Get-Product/{Id:int}")]

        public async Task<ActionResult<ServiceModel<ProductsDTO>>> GetProduct(int Id)
        {
            var result = await productRepo.GetProductById(Id);
            return Ok(result);
        }

        [HttpPost("Add-Product")]
        public async Task<ActionResult<ProductsDTO>> AddProduct(ProductsDTO product)
        {
            return Ok(await productRepo.AddProduct(product));
        }
        [HttpGet("Get-Products")]
        public async Task<ActionResult<ServiceModel<ProductsDTO>>> GetProducts()
        {
            var result = await productRepo.GetProducts();
            return Ok(result);
        }

        [HttpDelete("Delete-Product/{Id:int}")]
        public async Task<ActionResult<ServiceModel<ProductsDTO>>> DeleteProduct(int id)
        {
            return Ok(await productRepo.DeleteProduct(id));
        }
        //
        //[HttpPost("Update-Product/{Id:int}")]
        //public async Task<ActionResult<ServiceModel<ProductsDTO>>> UpdateProduct(int id, ProductsDTO product)
        //{
        //    return Ok(await productRepo.UpdateProduct(id,product));
        //}
    }
}
    
