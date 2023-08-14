namespace PersonalFinanceManagement.Tests.InMemory
{
    public class RepositoryBaseTests : IClassFixture<TestInMemoryFixture>
    {
        private readonly TestInMemoryFixture _fixture;

        public RepositoryBaseTests(TestInMemoryFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task RemoveAsync_ShouldReturnNull_WhenIdOutOfRange()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new RepositoryBase<Transaction>(context);
            var transactionCount = await repository.GetCountAsync();
            var item = new Transaction { Id = transactionCount + 1, };

            // Act
            var result = await repository.RemoveByIdAsync(item.Id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveAsync_ShouldReturnDeletedItem_WhenItemExist()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new RepositoryBase<Transaction>(context);
            var transactionCountBefore = await repository.GetCountAsync();
            var item = new Transaction { Id = transactionCountBefore, };

            // Act
            var result = await repository.RemoveByIdAsync(item.Id);
            var transactionCountAfter = await repository.GetCountAsync();

            // Assert
            Assert.Equal(item.Id, result.Id);
            Assert.Equal(transactionCountBefore - 1, transactionCountAfter);

        }

        [Fact]
        public async void GetPageAsync_ShouldReturnItems_WhenItemsExist()
        {
            // Arrange
            using var context = new PFMDbContext(_fixture.TestDbContextOptions);
            var repository = new RepositoryBase<Wallet>(context);
            var totalInDb = await repository.GetCountAsync();
            int pageSize = 3;
            var pageIndex = totalInDb / pageSize;
            var itemsOnLastPage = totalInDb % pageSize;

            // Act
            var page = await repository.GetPageAsync(pageIndex, pageSize);

            // Assert
            Assert.Equal(itemsOnLastPage, page.Items.Count());
        }
    }
}
