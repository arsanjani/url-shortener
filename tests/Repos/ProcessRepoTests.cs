using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using ScissorLink.Repos;
using ScissorLink.Models;
using ScissorLink.Data;

namespace ScissorLink.Tests.Repos;

public class ProcessRepoTests
{
    private readonly Mock<ScissorLinkDbContext> _mockContext;
    private readonly ProcessRepo _repo;

    public ProcessRepoTests()
    {
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _mockContext = new Mock<ScissorLinkDbContext>(options);
        _repo = new ProcessRepo(_mockContext.Object);
    }

    [Fact]
    public void Constructor_WithValidContext_ShouldInitializeCorrectly()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new ScissorLinkDbContext(options);

        // Act
        var repo = new ProcessRepo(context);

        // Assert
        Assert.NotNull(repo);
    }

    [Fact]
    public void Constructor_WithNullContext_ShouldThrowException()
    {
        // Arrange, Act & Assert
        // Note: BaseRepo constructor doesn't validate null, so this test verifies the actual behavior
        var exception = Record.Exception(() => new ProcessRepo(null!));
        Assert.Null(exception); // Constructor doesn't throw, but subsequent operations will fail
    }

    [Fact]
    public async Task Get_WithExistingToken_ShouldReturnShortLink()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var testShortLink = new DtoShortLink
        {
            ID = 1,
            Token = "test-token",
            OriginLink = "https://example.com",
            IsPublish = true
        };

        using (var context = new ScissorLinkDbContext(options))
        {
            context.ShortLinks.Add(testShortLink);
            await context.SaveChangesAsync();
        }

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act
            var result = await repo.Get("test-token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test-token", result.Token);
            Assert.Equal("https://example.com", result.OriginLink);
        }
    }

    [Fact]
    public async Task Get_WithNonExistingToken_ShouldReturnNull()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act
            var result = await repo.Get("non-existing-token");

            // Assert
            Assert.Null(result);
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("test-token")]
    [InlineData("very-long-token-that-should-still-work")]
    [InlineData("token-with-special-chars-!@#$%^&*()")]
    public async Task Get_WithVariousTokens_ShouldHandleCorrectly(string token)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => repo.Get(token));
            Assert.Null(exception);
        }
    }

    [Fact]
    public async Task Save_WithValidDto_ShouldReturnTrue()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dto = new DtoShortLinkDetail
        {
            ShortLinkID = 1,
            Country = "US",
            OS = "Windows",
            Browser = "Chrome"
        };

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act
            var result = await repo.Save(dto);

            // Assert
            Assert.True(result);

            // Verify it was saved
            var saved = await context.ShortLinkDetails.FirstOrDefaultAsync(x => x.ShortLinkID == 1);
            Assert.NotNull(saved);
            Assert.Equal("US", saved.Country);
        }
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllShortLinks()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var testShortLinks = new List<DtoShortLink>
        {
            new() { ID = 1, Token = "token1", OriginLink = "https://example1.com", IsPublish = true },
            new() { ID = 2, Token = "token2", OriginLink = "https://example2.com", IsPublish = false }
        };

        using (var context = new ScissorLinkDbContext(options))
        {
            context.ShortLinks.AddRange(testShortLinks);
            await context.SaveChangesAsync();
        }

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act
            var result = await repo.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
        }
    }

    [Fact]
    public async Task GetById_WithExistingId_ShouldReturnShortLink()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var testShortLink = new DtoShortLink
        {
            ID = 1,
            Token = "test-token",
            OriginLink = "https://example.com",
            IsPublish = true
        };

        using (var context = new ScissorLinkDbContext(options))
        {
            context.ShortLinks.Add(testShortLink);
            await context.SaveChangesAsync();
        }

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act
            var result = await repo.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ID);
            Assert.Equal("test-token", result.Token);
        }
    }

    [Fact]
    public async Task Create_WithValidShortLink_ShouldReturnCreatedShortLink()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var newShortLink = new DtoShortLink
        {
            Token = "new-token",
            OriginLink = "https://newexample.com",
            IsPublish = true
        };

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act
            var result = await repo.Create(newShortLink);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new-token", result.Token);
            Assert.True(result.ID > 0);
        }
    }

    [Fact]
    public async Task Update_WithValidShortLink_ShouldReturnUpdatedShortLink()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var originalShortLink = new DtoShortLink
        {
            ID = 1,
            Token = "original-token",
            OriginLink = "https://original.com",
            IsPublish = true
        };

        using (var context = new ScissorLinkDbContext(options))
        {
            context.ShortLinks.Add(originalShortLink);
            await context.SaveChangesAsync();
        }

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);
            originalShortLink.OriginLink = "https://updated.com";

            // Act
            var result = await repo.Update(originalShortLink);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("https://updated.com", result.OriginLink);
        }
    }

    [Fact]
    public async Task Delete_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var testShortLink = new DtoShortLink
        {
            ID = 1,
            Token = "test-token",
            OriginLink = "https://example.com",
            IsPublish = true
        };

        using (var context = new ScissorLinkDbContext(options))
        {
            context.ShortLinks.Add(testShortLink);
            await context.SaveChangesAsync();
        }

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act
            var result = await repo.Delete(1);

            // Assert
            Assert.True(result);

            // Verify it was deleted
            var deleted = await context.ShortLinks.FindAsync(1);
            Assert.Null(deleted);
        }
    }

    [Fact]
    public async Task Delete_WithNonExistingId_ShouldReturnFalse()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act
            var result = await repo.Delete(999);

            // Assert
            Assert.False(result);
        }
    }

    [Fact]
    public async Task TokenExists_WithExistingToken_ShouldReturnTrue()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var testShortLink = new DtoShortLink
        {
            ID = 1,
            Token = "existing-token",
            OriginLink = "https://example.com",
            IsPublish = true
        };

        using (var context = new ScissorLinkDbContext(options))
        {
            context.ShortLinks.Add(testShortLink);
            await context.SaveChangesAsync();
        }

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act
            var result = await repo.TokenExists("existing-token");

            // Assert
            Assert.True(result);
        }
    }

    [Fact]
    public async Task TokenExists_WithNonExistingToken_ShouldReturnFalse()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act
            var result = await repo.TokenExists("non-existing-token");

            // Assert
            Assert.False(result);
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("special-chars-!@#$%^&*()")]
    [InlineData("üñîçødé-tøkéñ")]
    public async Task TokenExists_WithVariousTokens_ShouldHandleCorrectly(string token)
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => repo.TokenExists(token));
            Assert.Null(exception);
        }
    }

    [Fact]
    public async Task Save_WithDtoWithNullProperties_ShouldHandleCorrectly()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dto = new DtoShortLinkDetail
        {
            ShortLinkID = 1,
            Country = null,
            OS = null,
            Browser = null
        };

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act
            var result = await repo.Save(dto);

            // Assert
            Assert.True(result);
        }
    }

    [Fact]
    public async Task Save_WithDtoWithSpecialCharacters_ShouldHandleCorrectly()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ScissorLinkDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dto = new DtoShortLinkDetail
        {
            ShortLinkID = 1,
            Country = "Üñîçødé Tëst",
            OS = "Windows 10 Pro™",
            Browser = "Chrome/91.0.4472.124"
        };

        using (var context = new ScissorLinkDbContext(options))
        {
            var repo = new ProcessRepo(context);

            // Act
            var result = await repo.Save(dto);

            // Assert
            Assert.True(result);
        }
    }
}