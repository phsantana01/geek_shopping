using GeekShopping.ProductAPI.Data.Dto;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Repository;
using GeekShopping.ProductAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new
                ArgumentException(nameof(productRepository));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> FindAll()
        {
            IEnumerable<ProductDto> products = await _productRepository.FindAll();

            if (products != null) return Ok(products);

            return NotFound();
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> FindById(long id) 
        {
            ProductDto product = await _productRepository.FindById(id);

            if (product != null)  return Ok(product);

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProductDto dto) 
        {
            if (dto != null) 
            {
                ProductDto produto = await _productRepository.Create(dto);
                return Ok(produto);
            }
            return BadRequest();
        }

        [HttpPut]
        [Authorize]

        public async Task<IActionResult> Update([FromBody] ProductDto dto) 
        {
            if (dto != null) 
            {
                await _productRepository.Update(dto);
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Role.ADMIN)]

        public async Task<IActionResult> Delete([FromRoute] long id) 
        {
            if (await _productRepository.Delete(id)) return NoContent();

            return NotFound() ;
        }
    }
}
