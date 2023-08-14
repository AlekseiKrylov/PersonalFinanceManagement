namespace PersonalFinanceManagement.Tests.InMemory
{
    public class CategoryRepositoryTests : IClassFixture<TestInMemoryFixture>
    {
        private readonly TestInMemoryFixture _fixture;

        public CategoryRepositoryTests(TestInMemoryFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnFalse_WhenWalletBelongsAnotherUser()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 1;
            var walletAnoterUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId + 1);
            var repository = new CategoryRepository(context);
            repository.SetUserId(userId);

            // Act
            var result = await repository.CheckEntitiesExistAsync(walletAnoterUser.Id, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnFalse_WhenCategoryBelongsAnotherUser()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 1;
            var categoryAnoterUser = await context.Categories.FirstOrDefaultAsync(c => c.Wallet.UserId == userId + 1);
            var walletUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var repository = new CategoryRepository(context);
            repository.SetUserId(userId);

            // Act
            var result = await repository.CheckEntitiesExistAsync(walletUser.Id, categoryAnoterUser.Id);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnTrue_WhenWalletBelongsUser()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 1;
            var walletUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var repository = new CategoryRepository(context);
            repository.SetUserId(userId);

            // Act
            var result = await repository.CheckEntitiesExistAsync(walletUser.Id, null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnTrue_WhenWalletAndCategoryBelongsUser()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 1;
            var walletUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var categoryUser = await context.Categories.FirstOrDefaultAsync(c => c.Wallet.UserId == userId);
            var repository = new CategoryRepository(context);
            repository.SetUserId(userId);

            // Act
            var result = await repository.CheckEntitiesExistAsync(walletUser.Id, categoryUser.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void GetPageWithRestrictionsAsync_ShouldReturnItems_WhenItemsExist()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            int userId = 1;
            var walletUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var repository = new CategoryRepository(context);
            repository.SetUserId(userId);
            var totalInDb = await context.Categories.CountAsync();
            var totalInUserWallet = await context.Categories.Where(c => c.WalletId == walletUser.Id).CountAsync();
            int pageSize = 3;
            var pageIndex = totalInUserWallet / pageSize;
            var itemsOnLastPage = totalInUserWallet % pageSize;

            // Act
            var page = await repository.GetPageWithRestrictionsAsync(pageIndex, pageSize, walletUser.Id);

            // Assert
            Assert.Equal(itemsOnLastPage, page.Items.Count());
            Assert.NotEqual(totalInDb, totalInUserWallet);
        }
    }
}
