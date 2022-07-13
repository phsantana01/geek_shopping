using AutoMapper;
using GeekShopping.ProductAPI.Data.Dto;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlServerContext _context;
        private IMapper _mapper;

        public ProductRepository(SqlServerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductDto>> FindAll()
        {
            List<Product> products = await  _context.Products.ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto> FindById(long id)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> Create(ProductDto dto)
        {
            Product product = _mapper.Map<Product>(dto);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> Update(ProductDto dto)
        {
            Product product = _mapper.Map<Product>(dto);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;

            }
        }
    }
}
