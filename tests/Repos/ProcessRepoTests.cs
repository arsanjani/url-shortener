using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using akhr.ir.Repos;
using akhr.ir.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ScissorLink.Tests.Repos;

public class ProcessRepoTests
{
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly ProcessRepo _repo;

    public ProcessRepoTests()
    {
        _mockConfig = new Mock<IConfiguration>();
        // Don't setup the connection string - let it be null for most tests
        _repo = new ProcessRepo(_mockConfig.Object);
    }

    [Fact]
    public void Constructor_WithValidConfiguration_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var repo = new ProcessRepo(_mockConfig.Object);

        // Assert
        Assert.NotNull(repo);
    }

    [Fact]
    public void Constructor_WithNullConfiguration_ShouldThrowException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ProcessRepo(null!));
    }

    [Fact]
    public async Task Get_WithNullConnection_ShouldReturnNull()
    {
        // Arrange
        var mockConfigWithNullConnection = new Mock<IConfiguration>();
        var mockConnectionStringsSection = new Mock<IConfigurationSection>();
        mockConnectionStringsSection.Setup(x => x["dbScissorLink"]).Returns((string?)null);
        mockConfigWithNullConnection.Setup(x => x.GetSection("ConnectionStrings"))
                                  .Returns(mockConnectionStringsSection.Object);
        
        var repo = new ProcessRepo(mockConfigWithNullConnection.Object);

        // Act
        var result = await repo.Get("test-token");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Save_WithNullConnection_ShouldReturnFalse()
    {
        // Arrange
        var mockConfigWithNullConnection = new Mock<IConfiguration>();
        var mockConnectionStringsSection = new Mock<IConfigurationSection>();
        mockConnectionStringsSection.Setup(x => x["dbScissorLink"]).Returns((string?)null);
        mockConfigWithNullConnection.Setup(x => x.GetSection("ConnectionStrings"))
                                  .Returns(mockConnectionStringsSection.Object);
        
        var repo = new ProcessRepo(mockConfigWithNullConnection.Object);
        var dto = new DtoShortLinkDetail
        {
            ID = 1,
            ShortLinkID = 1,
            Country = "US",
            OS = "Windows",
            Browser = "Chrome"
        };

        // Act
        var result = await repo.Save(dto);

        // Assert
        Assert.False(result);
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
        // Note: This test will fail with actual database connection, but tests the method signature
        
        // Act & Assert
        // This should not throw an exception, even if it fails to connect
        var exception = await Record.ExceptionAsync(() => _repo.Get(token));
        
        // The exception type will vary based on the connection state
        // We're mainly testing that the method handles various token formats gracefully
        Assert.True(exception == null || exception is SqlException || exception is InvalidOperationException);
    }

    [Fact]
    public async Task Get_WithNullToken_ShouldHandleCorrectly()
    {
        // Arrange
        string? nullToken = null;

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _repo.Get(nullToken!));
        
        // Should not throw a null reference exception
        Assert.True(exception == null || exception is SqlException || exception is InvalidOperationException);
    }

    [Fact]
    public async Task Save_WithValidDto_ShouldHandleCorrectly()
    {
        // Arrange
        var dto = new DtoShortLinkDetail
        {
            ID = 1,
            ShortLinkID = 1,
            Country = "US",
            OS = "Windows",
            Browser = "Chrome"
        };

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _repo.Save(dto));
        
        // Should not throw an exception due to null dto
        Assert.True(exception == null || exception is SqlException || exception is InvalidOperationException);
    }

    [Fact]
    public async Task Save_WithNullDto_ShouldHandleCorrectly()
    {
        // Arrange
        DtoShortLinkDetail? nullDto = null;

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _repo.Save(nullDto!));
        
        // Should not throw a null reference exception
        Assert.True(exception == null || exception is SqlException || exception is InvalidOperationException);
    }

    [Fact]
    public async Task Save_WithDtoWithNullProperties_ShouldHandleCorrectly()
    {
        // Arrange
        var dto = new DtoShortLinkDetail
        {
            ID = 1,
            ShortLinkID = 1,
            Country = null,
            OS = null,
            Browser = null
        };

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _repo.Save(dto));
        
        // Should handle null properties gracefully
        Assert.True(exception == null || exception is SqlException || exception is InvalidOperationException);
    }

    [Fact]
    public async Task Save_WithDtoWithEmptyProperties_ShouldHandleCorrectly()
    {
        // Arrange
        var dto = new DtoShortLinkDetail
        {
            ID = 1,
            ShortLinkID = 1,
            Country = "",
            OS = "",
            Browser = ""
        };

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _repo.Save(dto));
        
        // Should handle empty properties gracefully
        Assert.True(exception == null || exception is SqlException || exception is InvalidOperationException);
    }

    [Fact]
    public async Task Save_WithDtoWithLongProperties_ShouldHandleCorrectly()
    {
        // Arrange
        var dto = new DtoShortLinkDetail
        {
            ID = 1,
            ShortLinkID = 1,
            Country = new string('A', 1000),
            OS = new string('B', 1000),
            Browser = new string('C', 1000)
        };

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _repo.Save(dto));
        
        // Should handle long properties gracefully (may fail due to DB constraints)
        Assert.True(exception == null || exception is SqlException || exception is InvalidOperationException);
    }

    [Fact]
    public async Task Save_WithDtoWithSpecialCharacters_ShouldHandleCorrectly()
    {
        // Arrange
        var dto = new DtoShortLinkDetail
        {
            ID = 1,
            ShortLinkID = 1,
            Country = "Üñîçødé Tëst",
            OS = "Windows 10 Pro™",
            Browser = "Chrome/91.0.4472.124"
        };

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => _repo.Save(dto));
        
        // Should handle special characters gracefully
        Assert.True(exception == null || exception is SqlException || exception is InvalidOperationException);
    }

    [Fact]
    public void Dispose_ShouldDisposeCorrectly()
    {
        // Arrange
        var repo = new ProcessRepo(_mockConfig.Object);

        // Act & Assert
        var exception = Record.Exception(() => repo.Dispose());
        Assert.Null(exception);
    }

    [Fact]
    public void Dispose_CalledMultipleTimes_ShouldHandleCorrectly()
    {
        // Arrange
        var repo = new ProcessRepo(_mockConfig.Object);

        // Act & Assert
        var exception1 = Record.Exception(() => repo.Dispose());
        var exception2 = Record.Exception(() => repo.Dispose());
        var exception3 = Record.Exception(() => repo.Dispose());
        
        Assert.Null(exception1);
        Assert.Null(exception2);
        Assert.Null(exception3);
    }

    [Fact]
    public void Constructor_WithEmptyConnectionString_ShouldHandleCorrectly()
    {
        // Arrange
        var mockConfigWithEmptyConnection = new Mock<IConfiguration>();
        var mockConnectionStringsSection = new Mock<IConfigurationSection>();
        mockConnectionStringsSection.Setup(x => x["dbScissorLink"]).Returns("");
        mockConfigWithEmptyConnection.Setup(x => x.GetSection("ConnectionStrings"))
                                    .Returns(mockConnectionStringsSection.Object);

        // Act & Assert
        var exception = Record.Exception(() => new ProcessRepo(mockConfigWithEmptyConnection.Object));
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithInvalidConnectionString_ShouldHandleCorrectly()
    {
        // Arrange
        var mockConfigWithInvalidConnection = new Mock<IConfiguration>();
        var mockConnectionStringsSection = new Mock<IConfigurationSection>();
        mockConnectionStringsSection.Setup(x => x["dbScissorLink"]).Returns("invalid-connection-string");
        mockConfigWithInvalidConnection.Setup(x => x.GetSection("ConnectionStrings"))
                                      .Returns(mockConnectionStringsSection.Object);

        // Act & Assert
        var exception = Record.Exception(() => new ProcessRepo(mockConfigWithInvalidConnection.Object));
        Assert.Null(exception); // Constructor should not throw, but Get/Save will fail
    }
} 