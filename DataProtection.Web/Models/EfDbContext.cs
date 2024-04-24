using Microsoft.EntityFrameworkCore;

namespace DataProtection.Web.Models
{
    public class EfDbContext:DbContext
    {
       
        public EfDbContext(DbContextOptions options):base(options)
        {
            
        }

       
        public DbSet<Product> Products { get; set; }
    }
}
