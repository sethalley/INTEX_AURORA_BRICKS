using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace INTEX_AURORA_BRICKS.Models
{
    public class AuroraContext : IdentityDbContext
    {
        public AuroraContext(DbContextOptions<AuroraContext> options) : base(options)
        {
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<ItemBasedRecommendations> ItemBasedRecommendations { get; set; }

        public DbSet<Cart> Cart { get; set; }
    }


    //public class OrderContext : DbContext
    //{
    //    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    //    {
    //    }

    //    public DbSet<Order> Orders { get; set; }
    //}

    //public class CustomerContext : DbContext
    //{
    //    public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
    //    {
    //    }

    //    public DbSet<Customer> Customers { get; set; }
    //}

    //public class LineItemContext : DbContext
    //{
    //    public LineItemContext(DbContextOptions<LineItemContext> options) : base(options)
    //    {
    //    }

    //    public DbSet<LineItem> LineItems { get; set; }
    //}
}
