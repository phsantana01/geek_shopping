using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;
        public const string BASEPATH = "api/v1/product";

        public ProductService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<ProductModel>> FindAllProducts()
        {
            var response = await _client.GetAsync(BASEPATH);
            return await response.ReadContentAs<List<ProductModel>>();
        }

        public async Task<ProductModel> FindProductbyId(long id)
        {
            var response = await _client.GetAsync($"{BASEPATH}/{id}");
            return await response.ReadContentAs<ProductModel>();
        }

        public async Task<ProductModel> CreateProduct(ProductModel model)
        {
            var response = await _client.PostAsJson(BASEPATH, model);
            if (response.IsSuccessStatusCode) return await response.ReadContentAs<ProductModel>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<ProductModel> UpdateProduct(ProductModel model)
        {
            var response = await _client.PutAsJson(BASEPATH, model);
            if (response.IsSuccessStatusCode) return await response.ReadContentAs<ProductModel>();
            else throw new Exception("Something went wrong when calling API");
        }

        public async Task<bool> DeleteProductById(long id)
        {
            var response = await _client.DeleteAsync($"{BASEPATH}/{id}");
            if (response.IsSuccessStatusCode) return await response.ReadContentAs<bool>();
            else throw new Exception("Something went wrong when calling API");
        }


    }
}
