using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccess.DataContext
{
    //DbContext class represents the database i.e. an abstraction of the database
    //therefore in this class we're going to define anything related to the db
    //such as the tables, contraints, rules, relationships, etc
    
    public class ShoppingCartContext: IdentityDbContext<IdentityUser>
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options)
          : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } //note: represents the table Products
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
