using System;
using System.Reflection;
using Core.Entities;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data;


public class StoreContext(DbContextOptions options) : DbContext(options)
{
    // مثال على جدول المنتجات:
    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }
}



