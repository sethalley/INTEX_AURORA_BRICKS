using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

        public DbSet<UserRecommendations> UserRecommendations { get; set; }

        public DbSet<Order> Orders { get; set; }
        
        public DbSet<OrderPredictions> OrderPredictions { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<OrderPredictions>()
        //        .HasKey(op => op.transaction_ID); // TransactionId will be used as foreign key and part of composite key

        //    modelBuilder.Entity<OrderPredictions>()
        //        .Property(op => op.prediction)
        //        .IsRequired();

        //    modelBuilder.Entity<Order>()
        //        .HasOne(o => o.orderPrediction) // Each Order has one OrderPrediction
        //        .WithOne(op => op.Order)
        //        .HasForeignKey<OrderPredictions>(op => op.TransactionId)
        //        .IsRequired();
        //}

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
