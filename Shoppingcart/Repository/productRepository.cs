using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shoppingcart.Data;
using Shoppingcart.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppingcart.Repository
{
    public class productRepository: IproductRepository
    {
        
        
            private readonly UserDbContext _context;

            public productRepository(UserDbContext context)
            {
                _context = context;
            }

            
             public async Task<IEnumerable<Product>> GetAllProducts()
            {
                return await _context.Products.FromSqlRaw("EXEC GetAllProducts").ToListAsync();
            }

            
             public async Task<IEnumerable<Product>> GetProductById(int Id)
            {
                try
                {
                    var param = new SqlParameter("@Id", Id);

                    var productdet = await _context.Products
                        .FromSqlRaw(@"exec GetProductById @Id", param)
                        .ToListAsync();

                    return productdet;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred:Id not found");
                    throw;
                }
            }

           
            public async Task<bool> AddProduct(Product product)
            {
                try
                {
                var result = await _context.Database.ExecuteSqlRawAsync(
                    @"EXEC InsertProduct @ProductName, @Price, @Quantity, @Image",
                    new SqlParameter("@ProductName", product.ProductName),
                    new SqlParameter("@Price", product.Price),
                    new SqlParameter("@Quantity", product.Quantity),
                    new SqlParameter("@Image", product.Image));
                       


                    return result > 0;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }

              public async Task<Product> UpdateProduct(Product product)
              {
                try
                {
                await _context.Database.ExecuteSqlRawAsync(
                      @"EXEC UpdateProduct @Id @ProductName, @Price, @Quantity, @Image",
                    new SqlParameter("@Id", product.Id),
                    new SqlParameter("@ProductName", product.ProductName),
                    new SqlParameter("@Price", product.Price),
                    new SqlParameter("@Quantity", product.Quantity),
                    new SqlParameter("@Image", product.Image));
                       

                    return product;
                }
                catch (Exception ex)
                {

                    return null;
                }
            }

            
             public async Task<bool> DeleteProduct(int Id)
            {
                int i = await Task.Run(() => _context.Database.ExecuteSqlInterpolatedAsync($"DeleteProduct {Id}"));
                if (i == 1)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }


        }

    
}
