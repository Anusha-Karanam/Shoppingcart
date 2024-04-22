using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoppingcart.Data;
using Shoppingcart.Models;

namespace Shoppingcart.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly UserDbContext _context;

        public ProductsController(UserDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
       
         public async Task<ActionResult<IEnumerable<Product>>>GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{Id}")]
        
        public async Task<ActionResult<Product>> GetProductById(int Id)
        {
            var product = await _context.Products.FindAsync(Id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutProduct(int Id, Product product)
        {
            if (Id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductById", new { Id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProduct(int Id)
         {
            var product = await _context.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int Id)
        {
            return _context.Products.Any(e => e.Id == Id);
        }
    }
}
