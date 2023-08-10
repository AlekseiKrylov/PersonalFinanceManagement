using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.Domain.DALEntities;

namespace PersonalFinanceManagement.API.Data
{
    public class PFMDbInitializer
    {
        private readonly PFMDbContext _db;

        public PFMDbInitializer(PFMDbContext db)
        {
            _db = db;
        }

        public void Initialize()
        {
            _db.Database.Migrate();

            if (_db.Wallets.Any())
                return;

            FillTestData();
        }

        private void FillTestData()
        {
            var rnd = new Random();
            for (int i = 1; i <= 5; i++)
            {
                var user = new User
                {
                    Email = $"testuser{i}@testmail.com",
                    PasswordHash = "empty",
                    VerificationToken = Guid.NewGuid().ToString(),
                    VerifiedAt = DateTime.Now,
                };

                var wallet = new Wallet
                {
                    Name = $"Wallet {i}",
                    Description = $"Test wallet {i}",
                    User = user
                };
                _db.Wallets.Add(wallet);

                for (var (j, cCount) = (0, rnd.Next(4, 7)); j < cCount; j++)
                {
                    var category = new Category
                    {
                        Name = $"Category {j}",
                        IsIncome = rnd.Next(2) == 0,
                        Wallet = wallet
                    };
                    _db.Categories.Add(category);

                    var transactions = new Transaction[rnd.Next(5, 10)];
                    for (var (k, tCount) = (0, transactions.Length); k < tCount; k++)
                    {
                        var transaction = new Transaction
                        {
                            Name = $"Transaction {k}",
                            Note = $"Test transaction {k}",
                            Amount = Math.Round((decimal)rnd.NextDouble() * (10000 - 100) + 100, 2),
                            Date = DateTime.Now.AddDays(-rnd.Next(0, 365)),
                            Category = category,
                            Wallet = wallet
                        };
                        transactions[k] = transaction;
                    }
                    _db.Transactions.AddRange(transactions);
                }
            }
            _db.SaveChanges();
        }
    }
}
