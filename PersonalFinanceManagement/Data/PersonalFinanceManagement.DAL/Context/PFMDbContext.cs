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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var modifiedEntries = ChangeTracker.Entries<Transaction>()
                .Where(t => t.State == EntityState.Added || (t.State == EntityState.Modified && t.Property(nameof(Transaction.Amount)).IsModified));
            
            if (modifiedEntries.Any())
                foreach (var entry in modifiedEntries)
                    entry.Entity.Amount = Math.Abs(entry.Entity.Amount);

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries<Transaction>()
                .Where(t => t.State == EntityState.Added || (t.State == EntityState.Modified && t.Property(nameof(Transaction.Amount)).IsModified));

            if (modifiedEntries.Any())
                foreach (var entry in modifiedEntries)
                    entry.Entity.Amount = Math.Abs(entry.Entity.Amount);

            return base.SaveChanges();
        }
    }
}
