using Microsoft.EntityFrameworkCore;
using Stock.Service.Domain.Entities;
using Stock.Service.Persistence.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Service.Persistence.Context
{
    public class StockServiceContext :DbContext
    {

        public StockServiceContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var ids = new Guid[] { new Guid("df81c0d5-4628-4ff9-8ac7-44230236750f"), new Guid("f128319a-2981-4b55-ba98-bd0df16c482b") };
            modelBuilder.ApplyConfiguration(new OwnerContextConfiguration(ids));
            modelBuilder.ApplyConfiguration(new AccountContextConfiguration(ids));
        }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
