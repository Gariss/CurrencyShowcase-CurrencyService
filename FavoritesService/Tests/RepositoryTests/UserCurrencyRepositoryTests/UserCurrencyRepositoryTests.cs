using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FavoritesService.Database.Repositories;
using FavoritesService.Database;

namespace FavoritesService.Tests.RepositoryTests.UserCurrencyRepositoryTests;

[TestFixture]
public partial class UserCurrencyRepositoryTests
{
    private CurrencyShowcaseContext _dbContext;
    private Mock<ILogger<UserCurrencyRepository>> _loggerMock;
    private UserCurrencyRepository _repository;
    private DbContextOptions<CurrencyShowcaseContext> _dbContextOptions;

    [SetUp]
    public void Setup()
    {
        // Configure in-memory database for testing
        _dbContextOptions = new DbContextOptionsBuilder<CurrencyShowcaseContext>()
            .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
            .Options;

        _dbContext = new CurrencyShowcaseContext(_dbContextOptions);
        _loggerMock = new Mock<ILogger<UserCurrencyRepository>>();
        _repository = new UserCurrencyRepository(_dbContext, _loggerMock.Object);

        // Seed test data
        SeedTestData();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext?.Dispose();
    }

    private void SeedTestData()
    {
        // Add test currencies
        _dbContext.Currencies.AddRange(testCurrencies);
        // Add test user currencies
        _dbContext.UserCurrencies.AddRange(testUserCurrencies);
        _dbContext.SaveChanges();
    }

    [Test]
    public async Task GetByUserIdAsync_WhenUserHasFavorites_ReturnsCurrencies()
    {
        // Arrange
        var userId = userId1;
        string[] expectedCurrencyIds = [
            "R01235",
            "R01239",
            "R01035",
            "R01775",
            "R01350",
            "R01010",
            "R01530",
            "R01535",
            "R01770"];

        // Act
        var result = await _repository.GetByUserIdAsync(userId, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(9));
        Assert.That(expectedCurrencyIds, Is.EqualTo(result.Select(c => c.Id)));
    }

    [Test]
    public async Task GetByUserIdAsync_WhenUserHasNoFavorites_ReturnsEmptyList()
    {
        // Arrange
        var userId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        // Act
        var result = await _repository.GetByUserIdAsync(userId, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task AddByUserIdAsync_WithNewCurrencies_AddsToDatabase()
    {
        // Arrange
        var userId = userId4;
        string[] newCurrencyIds = [ "R01280", "R01300", "R01335" ];

        // Act
        await _repository.AddByUserIdAsync(userId, newCurrencyIds, CancellationToken.None);

        // Assert
        var userCurrencies = _dbContext.UserCurrencies
            .Where(uc => uc.UserId == userId)
            .ToArray();

        Assert.That(userCurrencies, Is.Not.Null);
        Assert.That(userCurrencies.Count, Is.EqualTo(9));
        Assert.That(userCurrencies.Select(x => x.CurrencyId), Is.SupersetOf(newCurrencyIds));
    }

    [Test]
    public async Task AddByUserIdAsync_WithExistingCurrencies_DoesNotAddDuplicates()
    {
        // Arrange
        var userId = userId1;
        
        string[] existingCurrencyIds = [ "R01235", "R01239" ]; // Already exists for this user

        // Act
        var beforeUserCurrencies = _dbContext.UserCurrencies
            .Where(uc => uc.UserId == userId)
            .ToArray();

        await _repository.AddByUserIdAsync(userId, existingCurrencyIds, CancellationToken.None);
        var afterUserCurrencies = _dbContext.UserCurrencies
            .Where(uc => uc.UserId == userId)
            .ToArray();

        // Assert
        Assert.That(beforeUserCurrencies, Is.Not.Null);
        Assert.That(afterUserCurrencies, Is.Not.Null);
        Assert.That(beforeUserCurrencies, Is.EquivalentTo(afterUserCurrencies)); // Db Set should not change
    }

    [Test]
    public async Task AddByUserIdAsync_WithMixedCurrencies_AddsOnlyNewOnes()
    {
        // Arrange
        var userId = userId2;
        string[] existingCurrencyIds = [
            "R01239", // exists
            "R01235", // exists
            ];

        string[] newCurrencyIds = [
            "R01395", // new
            "R01500", // new
            "R01503"  // new
            ];

        string[] mixedCurrencyIds = existingCurrencyIds.Union(newCurrencyIds).ToArray();

        // Act
        var beforeUserCurrencies = _dbContext.UserCurrencies
            .Where(uc => uc.UserId == userId)
            .ToList();

        await _repository.AddByUserIdAsync(userId, mixedCurrencyIds, CancellationToken.None);

        var afterUserCurrencies = _dbContext.UserCurrencies
            .Where(uc => uc.UserId == userId)
            .ToList();

        // Assert
        Assert.That(beforeUserCurrencies, Is.Not.Null);
        Assert.That(afterUserCurrencies, Is.Not.Null);

        var actualNewIds = afterUserCurrencies.Select(x => x.CurrencyId)
            .Except(beforeUserCurrencies.Select(x => x.CurrencyId)).ToArray();

        Assert.That(actualNewIds, Is.EquivalentTo(newCurrencyIds)); // exact difference
    }

    [Test]
    public async Task RemoveByUserIdAsync_WithExistingCurrencies_RemovesFromDatabase()
    {
        // Arrange
        var userId = userId2;
        string[] existingCurrencyIds = [
            "R01239", // exists
            "R01235", // exists
            ];

        // Act
        var beforeUserCurrencies = _dbContext.UserCurrencies
            .Where(uc => uc.UserId == userId)
            .ToList();

        await _repository.RemoveByUserIdAsync(userId, existingCurrencyIds, CancellationToken.None);

        var afterUserCurrencies = _dbContext.UserCurrencies
            .Where(uc => uc.UserId == userId)
            .ToList();

        // Assert
        Assert.That(beforeUserCurrencies, Is.Not.Null);
        Assert.That(afterUserCurrencies, Is.Not.Null);

        var actualDeletedIds = beforeUserCurrencies.Select(x => x.CurrencyId)
            .Except(afterUserCurrencies.Select(x => x.CurrencyId)).ToArray();

        Assert.That(actualDeletedIds, Is.EquivalentTo(existingCurrencyIds)); // exact difference
    }

    [Test]
    public async Task RemoveByUserIdAsync_WithNonExistingCurrencies_DoesNothing()
    {
        // Arrange
        var userId = userId2;

        string[] newCurrencyIds = [
            "R01395", // new
            "R01500", // new
            "R01503"  // new
            ];

        // Act
        var beforeUserCurrencies = _dbContext.UserCurrencies
            .Where(uc => uc.UserId == userId)
            .ToList();

        await _repository.RemoveByUserIdAsync(userId, newCurrencyIds, CancellationToken.None);

        var afterUserCurrencies = _dbContext.UserCurrencies
            .Where(uc => uc.UserId == userId)
            .ToList();

        // Assert
        Assert.That(beforeUserCurrencies, Is.Not.Null);
        Assert.That(afterUserCurrencies, Is.Not.Null);

        Assert.That(beforeUserCurrencies, Is.EquivalentTo(afterUserCurrencies)); // Db Set should not change
    }

    [Test]
    public async Task RemoveByUserIdAsync_WithMixedCurrencies_RemovesOnlyExistingOnes()
    {
        // Arrange
        var userId = userId2;
        string[] existingCurrencyIds = [
            "R01239", // exists
            "R01235", // exists
            ];

        string[] newCurrencyIds = [
            "R01395", // new
            "R01500", // new
            "R01503"  // new
            ];

        string[] mixedCurrencyIds = existingCurrencyIds.Union(newCurrencyIds).ToArray();

        // Act
        var beforeUserCurrencies = _dbContext.UserCurrencies
            .Where(uc => uc.UserId == userId)
            .ToList();

        await _repository.RemoveByUserIdAsync(userId, mixedCurrencyIds, CancellationToken.None);

        var afterUserCurrencies = _dbContext.UserCurrencies
            .Where(uc => uc.UserId == userId)
            .ToList();

        // Assert
        Assert.That(beforeUserCurrencies, Is.Not.Null);
        Assert.That(afterUserCurrencies, Is.Not.Null);

        var actualDeletedIds = beforeUserCurrencies.Select(x => x.CurrencyId)
            .Except(afterUserCurrencies.Select(x => x.CurrencyId)).ToArray();

        Assert.That(actualDeletedIds, Is.EquivalentTo(existingCurrencyIds)); // exact difference
    }
}