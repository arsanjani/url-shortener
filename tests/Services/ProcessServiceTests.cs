using Xunit;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using ScissorLink.Services;
using ScissorLink.Services.Interface;
using ScissorLink.Repos.Interface;
using ScissorLink.Models;

namespace ScissorLink.Tests.Services;

public class ProcessServiceTests
{
    private readonly Mock<IProcessRepo> _mockRepo;
    private readonly Mock<IMemoryCache> _mockCache;
    private readonly ProcessService _service;

    public ProcessServiceTests()
    {
        _mockRepo = new Mock<IProcessRepo>();
        _mockCache = new Mock<IMemoryCache>();
        
        // Setup default cache behavior
        _mockCache.Setup(c => c.CreateEntry(It.IsAny<object>()))
                  .Returns(Mock.Of<ICacheEntry>());
        
        _service = new ProcessService(_mockRepo.Object, _mockCache.Object);
    }

    [Fact]
    public async Task Get_WithCacheHit_ShouldReturnCachedResult()
    {
        // Arrange
        var token = "test-token";
        var expectedResult = new DtoShortLink
        {
            ID = 1,
            Token = token,
            OriginLink = "https://example.com",
            IsPublish = true
        };

        object? cachedValue = expectedResult;
        _mockCache.Setup(c => c.TryGetValue($"ShortLink-{token}", out cachedValue))
                  .Returns(true);

        // Act
        var result = await _service.Get(token);

        // Assert
        Assert.Equal(expectedResult, result);
        _mockRepo.Verify(r => r.Get(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Get_WithCacheMiss_ShouldCallRepository()
    {
        // Arrange
        var token = "test-token";
        var expectedResult = new DtoShortLink
        {
            ID = 1,
            Token = token,
            OriginLink = "https://example.com",
            IsPublish = true
        };

        object? cachedValue = null;
        _mockCache.Setup(c => c.TryGetValue($"ShortLink-{token}", out cachedValue))
                  .Returns(false);

        _mockRepo.Setup(r => r.Get(token))
                 .ReturnsAsync(expectedResult);

        // Act
        var result = await _service.Get(token);

        // Assert
        Assert.Equal(expectedResult, result);
        _mockRepo.Verify(r => r.Get(token), Times.Once);
        _mockCache.Verify(c => c.CreateEntry($"ShortLink-{token}"), Times.Once);
    }

    [Fact]
    public async Task Get_WithCacheMissAndNullResult_ShouldNotCache()
    {
        // Arrange
        var token = "test-token";
        DtoShortLink? nullResult = null;

        object? cachedValue = null;
        _mockCache.Setup(c => c.TryGetValue($"ShortLink-{token}", out cachedValue))
                  .Returns(false);

        _mockRepo.Setup(r => r.Get(token))
                 .ReturnsAsync(nullResult);

        // Act
        var result = await _service.Get(token);

        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.Get(token), Times.Once);
        _mockCache.Verify(c => c.CreateEntry(It.IsAny<object>()), Times.Never);
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
        var expectedResult = new DtoShortLink
        {
            ID = 1,
            Token = token,
            OriginLink = "https://example.com",
            IsPublish = true
        };

        object? cachedValue = null;
        _mockCache.Setup(c => c.TryGetValue($"ShortLink-{token}", out cachedValue))
                  .Returns(false);

        _mockRepo.Setup(r => r.Get(token))
                 .ReturnsAsync(expectedResult);

        // Act
        var result = await _service.Get(token);

        // Assert
        Assert.Equal(expectedResult, result);
        _mockRepo.Verify(r => r.Get(token), Times.Once);
    }

    [Fact]
    public async Task Get_WithNullToken_ShouldCallRepositoryWithNull()
    {
        // Arrange
        string? nullToken = null;
        DtoShortLink? nullResult = null;

        object? cachedValue = null;
        _mockCache.Setup(c => c.TryGetValue(It.IsAny<string>(), out cachedValue))
                  .Returns(false);

        _mockRepo.Setup(r => r.Get(nullToken!))
                 .ReturnsAsync(nullResult);

        // Act
        var result = await _service.Get(nullToken!);

        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.Get(nullToken!), Times.Once);
    }

    [Fact]
    public async Task Save_ShouldCallRepository()
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

        _mockRepo.Setup(r => r.Save(dto))
                 .ReturnsAsync(true);

        // Act
        var result = await _service.Save(dto);

        // Assert
        Assert.True(result);
        _mockRepo.Verify(r => r.Save(dto), Times.Once);
    }

    [Fact]
    public async Task Save_WithNullDto_ShouldCallRepositoryWithNull()
    {
        // Arrange
        DtoShortLinkDetail? nullDto = null;

        _mockRepo.Setup(r => r.Save(nullDto!))
                 .ReturnsAsync(false);

        // Act
        var result = await _service.Save(nullDto!);

        // Assert
        Assert.False(result);
        _mockRepo.Verify(r => r.Save(nullDto!), Times.Once);
    }

    [Fact]
    public async Task Save_WithRepositoryFailure_ShouldReturnFalse()
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

        _mockRepo.Setup(r => r.Save(dto))
                 .ReturnsAsync(false);

        // Act
        var result = await _service.Save(dto);

        // Assert
        Assert.False(result);
        _mockRepo.Verify(r => r.Save(dto), Times.Once);
    }

    [Fact]
    public async Task Get_WithRepositoryException_ShouldThrowException()
    {
        // Arrange
        var token = "test-token";
        var expectedException = new Exception("Database error");

        object? cachedValue = null;
        _mockCache.Setup(c => c.TryGetValue($"ShortLink-{token}", out cachedValue))
                  .Returns(false);

        _mockRepo.Setup(r => r.Get(token))
                 .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Get(token));
        Assert.Equal(expectedException.Message, exception.Message);
    }

    [Fact]
    public async Task Save_WithRepositoryException_ShouldThrowException()
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
        var expectedException = new Exception("Database error");

        _mockRepo.Setup(r => r.Save(dto))
                 .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Save(dto));
        Assert.Equal(expectedException.Message, exception.Message);
    }

    [Fact]
    public async Task Get_WithCacheException_ShouldStillCallRepository()
    {
        // Arrange
        var token = "test-token";
        var expectedResult = new DtoShortLink
        {
            ID = 1,
            Token = token,
            OriginLink = "https://example.com",
            IsPublish = true
        };

        object? cachedValue = null;
        _mockCache.Setup(c => c.TryGetValue($"ShortLink-{token}", out cachedValue))
                  .Throws(new Exception("Cache error"));

        _mockRepo.Setup(r => r.Get(token))
                 .ReturnsAsync(expectedResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Get(token));
        Assert.Equal("Cache error", exception.Message);
    }

    [Fact]
    public async Task Get_WithMultipleConcurrentRequests_ShouldHandleCorrectly()
    {
        // Arrange
        var token = "test-token";
        var expectedResult = new DtoShortLink
        {
            ID = 1,
            Token = token,
            OriginLink = "https://example.com",
            IsPublish = true
        };

        object? cachedValue = null;
        _mockCache.Setup(c => c.TryGetValue($"ShortLink-{token}", out cachedValue))
                  .Returns(false);

        _mockRepo.Setup(r => r.Get(token))
                 .ReturnsAsync(expectedResult);

        // Act
        var tasks = new List<Task<DtoShortLink?>>();
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(_service.Get(token));
        }

        var results = await Task.WhenAll(tasks);

        // Assert
        Assert.All(results, result => Assert.Equal(expectedResult, result));
        _mockRepo.Verify(r => r.Get(token), Times.Exactly(10));
    }
} 