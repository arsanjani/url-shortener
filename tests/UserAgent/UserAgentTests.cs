using Xunit;
using ScissorLink.UserAgent;

namespace ScissorLink.Tests.UserAgent;

public class UserAgentTests
{
    [Fact]
    public void Constructor_WithNullUserAgent_ShouldNotThrow()
    {
        // Arrange & Act & Assert
        var exception = Record.Exception(() => new ScissorLink.UserAgent.UserAgent(null!));
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithEmptyUserAgent_ShouldNotThrow()
    {
        // Arrange & Act & Assert
        var exception = Record.Exception(() => new ScissorLink.UserAgent.UserAgent(""));
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithValidUserAgent_ShouldInitializeProperties()
    {
        // Arrange
        var userAgentString = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";

        // Act
        var userAgent = new ScissorLink.UserAgent.UserAgent(userAgentString);

        // Assert
        Assert.NotNull(userAgent.Browser);
        Assert.NotNull(userAgent.OS);
    }

    [Fact]
    public void Browser_ShouldReturnSameInstance_OnMultipleCalls()
    {
        // Arrange
        var userAgentString = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
        var userAgent = new ScissorLink.UserAgent.UserAgent(userAgentString);

        // Act
        var browser1 = userAgent.Browser;
        var browser2 = userAgent.Browser;

        // Assert
        Assert.Same(browser1, browser2);
    }

    [Fact]
    public void OS_ShouldReturnSameInstance_OnMultipleCalls()
    {
        // Arrange
        var userAgentString = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
        var userAgent = new ScissorLink.UserAgent.UserAgent(userAgentString);

        // Act
        var os1 = userAgent.OS;
        var os2 = userAgent.OS;

        // Assert
        Assert.Same(os1, os2);
    }
} 