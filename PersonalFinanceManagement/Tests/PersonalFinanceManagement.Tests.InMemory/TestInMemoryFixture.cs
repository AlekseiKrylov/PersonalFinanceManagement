namespace PersonalFinanceManagement.Tests.InMemory
{
    public class TestInMemoryFixture : IAsyncLifetime
    {
        private const int _USERS_COUNT = 2;
        private const int _WALLETS_PER_USER_COUNT = 2;
        private const int _INCOME_CATEGORIES_COUNT = 2;
        private const int _EXPENSE_CATEGORIES_COUNT = 2;
        private const int _TRANSACTIONS_PER_CATEGORY_COUNT = 5;

        public DbContextOptions<PFMDbContext> TestDbContextOptions { get; private set; }
        public PFMDbContext TestDbContext { get; private set; }

        public async Task InitializeAsync()
        {
            var dbSuffix = Guid.NewGuid().ToString();
            TestDbContextOptions = new DbContextOptionsBuilder<PFMDbContext>()
                .UseInMemoryDatabase($"PFMTest_DB_{dbSuffix}")
                .Options;

            TestDbContext = new PFMDbContext(TestDbContextOptions);

            await CreateUsersAsync(_USERS_COUNT);

            var users = TestDbContext.Users.ToList();
            foreach (var user in users)
                await CreateWalletsAsync(_WALLETS_PER_USER_COUNT, user);

            var wallets = TestDbContext.Wallets.ToList();
            foreach (var wallet in wallets)
                await CreateCategoriesAsync(_INCOME_CATEGORIES_COUNT, true, wallet);
            foreach (var wallet in wallets)
                await CreateCategoriesAsync(_EXPENSE_CATEGORIES_COUNT, false, wallet);

            var categories = TestDbContext.Categories.ToList();
            foreach (var category in categories)
                await CreateTransactionsAsync(_TRANSACTIONS_PER_CATEGORY_COUNT, category);
        }

        public Task DisposeAsync()
        {
            TestDbContext.Dispose();
            return Task.CompletedTask;
        }

        private async Task CreateUsersAsync(int usersCount)
        {
            var _users = Enumerable.Range(1, usersCount)
                .Select(i => new User
                {
                    Email = $"testuser{i}@testmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("string", BCrypt.Net.BCrypt.GenerateSalt()),
                    VerifiedAt = DateTime.Now,
                })
                .ToArray();

            await TestDbContext.Users.AddRangeAsync(_users);
            await TestDbContext.SaveChangesAsync();
        }

        private async Task CreateWalletsAsync(int walletsCount, User user)
        {
            var wallets = Enumerable.Range(1, walletsCount)
                .Select(i => new Wallet
                {
                    Name = $"Wallet{i}_User{user.Id}",
                    UserId = user.Id,
                })
                .ToArray();

            await TestDbContext.Wallets.AddRangeAsync(wallets);
            await TestDbContext.SaveChangesAsync();
        }

        private async Task CreateCategoriesAsync(int categoriesCount, bool isIncome, Wallet wallet)
        {
            var categories = Enumerable.Range(1, categoriesCount)
                .Select(i => new Category
                {
                    Name = $"Category{i}_{wallet.Name}",
                    IsIncome = isIncome,
                    WalletId = wallet.Id,
                })
                .ToArray();

            await TestDbContext.Categories.AddRangeAsync(categories);
            await TestDbContext.SaveChangesAsync();
        }

        private async Task CreateTransactionsAsync(int transactionsCount, Category category)
        {
            Random rnd = new();
            var transactions = Enumerable.Range(1, transactionsCount)
                .Select(i => new Transaction
                {
                    Name = $"Transaction{i}_{category.Name}",
                    Amount = category.IsIncome ? rnd.Next(50, 500) : rnd.Next(20, 200),
                    Date = DateTime.Now.Date.AddDays(-i),
                    CategoryId = category.Id,
                    //WalletId = category.WalletId,
                })
                .ToArray();

            await TestDbContext.Transactions.AddRangeAsync(transactions);
            await TestDbContext.SaveChangesAsync();
        }
    }
}