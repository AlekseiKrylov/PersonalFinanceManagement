namespace PersonalFinanceManagement.Tests.InMemory
{
    public class TransactionRepositoryTests : IClassFixture<TestInMemoryFixture>
    {
        private readonly TestInMemoryFixture _fixture;

        public TransactionRepositoryTests(TestInMemoryFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task GetPeriodWithCategoryAsync_ShouldReturnEmpty_WhenStartDateLaterThanEndDate()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new TransactionRepository(context);
            int walletId = 1;
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now.Date.AddDays(-1);

            // Act
            var result = await repository.GetPeriodWithCategoryAsync(walletId, startDate, endDate);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPeriodWithCategoryAsync_ShouldReturnReport_WhenStartDateEarlierThanEndDate()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 2;
            var repository = new TransactionRepository(context);
            repository.SetUserId(userId);
            var walletUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var totalInUserWallet = await context.Transactions.Where(t => t.Category.WalletId == walletUser.Id).CountAsync();
            var startDate = DateTime.Now.Date.AddDays(-1);
            var endDate = DateTime.Now.Date;

            // Act
            var result = await repository.GetPeriodWithCategoryAsync(walletUser.Id, startDate, endDate);

            // Assert
            Assert.NotEqual(totalInUserWallet, result.Count());
        }

        [Fact]
        public async Task MoveToAnotherCategoryAsync_ShouldReturnTrue_WhenCategoriesBelongsToSameWallet()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 1;
            var repository = new TransactionRepository(context);
            repository.SetUserId(userId);
            var walletUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var sourceCategory = await context.Categories.FirstOrDefaultAsync(c => c.WalletId == walletUser.Id && c.IsIncome == true);
            var targetCategory = await context.Categories.FirstOrDefaultAsync(c => c.WalletId == walletUser.Id && c.IsIncome == false);
            var transactionsInSourceCategoryBefore = await repository.GetCountInCategoryAsync(sourceCategory.Id);

            // Act
            var result = await repository.MoveToAnotherCategoryOfSameWalletAsync(walletUser.Id, sourceCategory.Id, targetCategory.Id);
            var transactionsInSourceCategoryAfter = await repository.GetCountInCategoryAsync(sourceCategory.Id);

            // Assert
            Assert.True(result);
            Assert.NotEqual(0, transactionsInSourceCategoryBefore);
            Assert.Equal(0, transactionsInSourceCategoryAfter);
        }

        [Fact]
        public async Task MoveToAnotherCategoryAsync_ShouldReturnFalse_WhenCategoriesBelongsToDifferentWallet()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 1;
            var repository = new TransactionRepository(context);
            repository.SetUserId(userId);
            var walletUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var sourceCategory = await context.Categories.FirstOrDefaultAsync(c => c.WalletId == walletUser.Id && c.IsIncome == false);
            var targetCategory = await context.Categories.FirstOrDefaultAsync(c => c.WalletId != walletUser.Id && c.IsIncome == false);
            var transactionsInSourceCategoryBefore = await repository.GetCountInCategoryAsync(sourceCategory.Id);

            // Act
            var result = await repository.MoveToAnotherCategoryOfSameWalletAsync(walletUser.Id, sourceCategory.Id, targetCategory.Id);
            var transactionsInSourceCategoryAfter = await repository.GetCountInCategoryAsync(sourceCategory.Id);

            // Assert
            Assert.False(result);
            Assert.Equal(transactionsInSourceCategoryBefore, transactionsInSourceCategoryAfter);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnFalse_WhenCategoryBelongsToDifferentUser()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 1;
            var repository = new TransactionRepository(context);
            repository.SetUserId(userId);
            var wrongCategory = await context.Categories.FirstOrDefaultAsync(c => c.Wallet.UserId != userId);

            // Act
            var result = await repository.CheckEntitiesExistAsync(wrongCategory.Id, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnFalse_WhenTransactionBelongsToDifferentUser()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 1;
            var repository = new TransactionRepository(context);
            repository.SetUserId(userId);
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Wallet.UserId == userId);
            var wrongTransaction = await context.Transactions.FirstOrDefaultAsync(t => t.Category.Wallet.UserId != userId);

            // Act
            var result = await repository.CheckEntitiesExistAsync(category.Id, wrongTransaction.Id);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnFalse_WhenTransactionBelongsToDifferentCategory()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 1;
            var repository = new TransactionRepository(context);
            repository.SetUserId(userId);
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Wallet.UserId == userId);
            var wrongTransaction = await context.Transactions.FirstOrDefaultAsync(t => t.Category.Wallet.UserId == userId && t.Category.Id != category.Id);

            // Act
            var result = await repository.CheckEntitiesExistAsync(category.Id, wrongTransaction.Id);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnTrue_WhenCategoryBelongsToUser()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 1;
            var repository = new TransactionRepository(context);
            repository.SetUserId(userId);
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Wallet.UserId == userId);

            // Act
            var result = await repository.CheckEntitiesExistAsync(category.Id, null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnTrue_WhenTransactionBelongsToCategory()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 1;
            var repository = new TransactionRepository(context);
            repository.SetUserId(userId);
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Wallet.UserId == userId);
            var transaction = await context.Transactions.FirstOrDefaultAsync(t => t.Category.Wallet.UserId == userId && t.Category.Id == category.Id);

            // Act
            var result = await repository.CheckEntitiesExistAsync(category.Id, transaction.Id);

            // Assert
            Assert.True(result);
        }
    }
}
