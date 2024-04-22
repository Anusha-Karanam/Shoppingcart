using Microsoft.EntityFrameworkCore;
using Shoppingcart.Models;

namespace Shoppingcart.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
       : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            
            
            
        }

    }
}
