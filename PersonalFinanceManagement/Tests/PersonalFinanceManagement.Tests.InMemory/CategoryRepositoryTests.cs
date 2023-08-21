namespace PersonalFinanceManagement.Tests.InMemory
{
    public class CategoryRepositoryTests : IClassFixture<TestInMemoryFixture>
    {
        private readonly TestInMemoryFixture _fixture;
        private readonly Mock<ICurrentUserService> _currencyUserServiceMock;

        public CategoryRepositoryTests(TestInMemoryFixture fixture)
        {
            _fixture = fixture;
            _currencyUserServiceMock = new Mock<ICurrentUserService>();
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnFalse_WhenWalletBelongsAnotherUser()
        {
            // Arrange
            int userId = 1;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var walletAnoterUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId + 1);
            var repository = new CategoryRepository(context, _currencyUserServiceMock.Object);

            // Act
            var result = await repository.CheckEntitiesExistAsync(walletAnoterUser.Id, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnFalse_WhenCategoryBelongsAnotherUser()
        {
            // Arrange
            int userId = 1;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var categoryAnoterUser = await context.Categories.FirstOrDefaultAsync(c => c.Wallet.UserId == userId + 1);
            var walletUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var repository = new CategoryRepository(context, _currencyUserServiceMock.Object);

            // Act
            var result = await repository.CheckEntitiesExistAsync(walletUser.Id, categoryAnoterUser.Id);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnTrue_WhenWalletBelongsUser()
        {
            // Arrange
            int userId = 1;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var walletUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var repository = new CategoryRepository(context, _currencyUserServiceMock.Object);

            // Act
            var result = await repository.CheckEntitiesExistAsync(walletUser.Id, null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckEntitiesExistAsync_ShouldReturnTrue_WhenWalletAndCategoryBelongsUser()
        {
            int userId = 1;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var walletUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var categoryUser = await context.Categories.FirstOrDefaultAsync(c => c.Wallet.UserId == userId);
            var repository = new CategoryRepository(context, _currencyUserServiceMock.Object);

            // Act
            var result = await repository.CheckEntitiesExistAsync(walletUser.Id, categoryUser.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void GetPageWithRestrictionsAsync_ShouldReturnItems_WhenItemsExist()
        {
            int userId = 1;
            _currencyUserServiceMock.Setup(s => s.GetCurretUserId()).Returns(userId);

            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var walletUser = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            var repository = new CategoryRepository(context, _currencyUserServiceMock.Object);
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
