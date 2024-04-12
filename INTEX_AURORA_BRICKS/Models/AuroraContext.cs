using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace INTEX_AURORA_BRICKS.Models
{
    public class AuroraContext : IdentityDbContext<Customers>
    {
        public AuroraContext(DbContextOptions<AuroraContext> options) : base(options)
        {
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<ItemBasedRecommendations> ItemBasedRecommendations { get; set; }

        public DbSet<Cart> Cart { get; set; }
        public DbSet<Customers> Customers { get; set; }
       

        public DbSet<UserRecommendations> UserRecommendations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Calling parent class (super) to configure Identity tables.
            base.OnModelCreating(modelBuilder);
        }

    }


}

