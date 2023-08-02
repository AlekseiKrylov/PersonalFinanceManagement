using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.DAL.Entities;

namespace PersonalFinanceManagement.DAL.Context
{
    public class PFMDbContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public PFMDbContext(DbContextOptions<PFMDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Wallet>()
                .HasMany<Transaction>()
                .WithOne(c => c.Wallet)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
