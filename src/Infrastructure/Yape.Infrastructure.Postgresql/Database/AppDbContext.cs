using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Yape.Infrastructure.Postgresql.Configuration;

namespace Yape.Infrastructure.Postgresql.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransactionEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}
