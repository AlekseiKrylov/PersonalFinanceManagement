namespace PersonalFinanceManagement.Tests.InMemory
{
    public class TransactionRepositoryTests : IClassFixture<TestInMemoryFixture>
    {
        private readonly TestInMemoryFixture _fixture;
        private readonly Mock<ICurrentUserService> _currencyUserServiceMock;

        public TransactionRepositoryTests(TestInMemoryFixture fixture)
        {
            _fixture = fixture;
            _currencyUserServiceMock = new Mock<ICurrentUserService>();
        }

        [Fact]
        public async Task GetPeriodWithCategoryAsync_ShouldReturnEmpty_WhenStartDateLaterThanEndDate()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new TransactionsRepository(context, _currencyUserServiceMock.Object);
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
            int userId = 2;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new TransactionsRepository(context, _currencyUserServiceMock.Object);
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
            int userId = 1;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new TransactionsRepository(context, _currencyUserServiceMock.Object);
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
            int userId = 1;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new TransactionsRepository(context, _currencyUserServiceMock.Object);
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
            int userId = 1;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new TransactionsRepository(context, _currencyUserServiceMock.Object);
            var wrongCategory = await context.Categories.FirstOrDefaultAsync(c => c.Wallet.UserId != userId);

            // Act
            var result = await repository.CheckEntitiesExistAsync(wrongCategory.Id, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnFalse_WhenTransactionBelongsToDifferentUser()
        {
            int userId = 1;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new TransactionsRepository(context, _currencyUserServiceMock.Object);
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
            int userId = 1;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new TransactionsRepository(context, _currencyUserServiceMock.Object);
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
            int userId = 1;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new TransactionsRepository(context, _currencyUserServiceMock.Object);
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Wallet.UserId == userId);

            // Act
            var result = await repository.CheckEntitiesExistAsync(category.Id, null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnTrue_WhenTransactionBelongsToCategory()
        {
            int userId = 1;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new TransactionsRepository(context, _currencyUserServiceMock.Object);
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Wallet.UserId == userId);
            var transaction = await context.Transactions.FirstOrDefaultAsync(t => t.Category.Wallet.UserId == userId && t.Category.Id == category.Id);

            // Act
            var result = await repository.CheckEntitiesExistAsync(category.Id, transaction.Id);

            // Assert
            Assert.True(result);
        }
    }
}
