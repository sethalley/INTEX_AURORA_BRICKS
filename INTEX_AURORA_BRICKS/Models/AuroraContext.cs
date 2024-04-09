using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace INTEX_AURORA_BRICKS.Models
{
    public class AuroraContext : IdentityDbContext<AuroraUser, IdentityRole, string>
    {
        public AuroraContext(DbContextOptions<AuroraContext> options) : base(options)
        {
        }




        public DbSet<Product> Products { get; set; }
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
