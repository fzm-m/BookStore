using System.Net.Http.Json;

namespace BookNook.Client.services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private readonly UserStateService _userState;

        public ProductService(HttpClient httpClient, UserStateService userState)
        {
            _httpClient = httpClient;
            _userState = userState;
        }

        // 获取所有产品
        public async Task<List<Product>> GetProductsAsync()
        {
            SetAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<List<Product>>("api/products");
        }

        // 新增产品
        public async Task CreateProductAsync(Product product)
        {
            SetAuthorizationHeader();
            await _httpClient.PostAsJsonAsync("api/products", product);
        }

        // 设置请求头中的JWT令牌
        private void SetAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_userState.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _userState.Token);
            }
        }
    }
}
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}