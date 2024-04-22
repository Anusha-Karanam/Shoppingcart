using Shoppingcart.Models;

namespace Shoppingcart.Repository
{
    public interface IproductRepository
    {
       
        
        public Task <bool>AddProduct(Product product);
        public Task<Product> UpdateProduct(Product product);
        public Task <bool>DeleteProduct(int Id);
        public Task<IEnumerable<Product>> GetAllProducts();
        public Task<IEnumerable<Product>> GetProductById(int Id);

    }
}
