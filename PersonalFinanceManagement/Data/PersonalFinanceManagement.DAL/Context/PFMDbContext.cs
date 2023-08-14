using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Domain.DALEntities;

namespace PersonalFinanceManagement.DAL.Context
{
    public class PFMDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
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

            modelBuilder.Entity<Category>()
                .HasIndex(c => new { c.Name, c.WalletId })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.VerificationToken)
                .IsUnique();
        }
    }
}
