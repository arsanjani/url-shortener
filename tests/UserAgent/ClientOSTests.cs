using Xunit;
using akhr.ir.UserAgent;

namespace ScissorLink.Tests.UserAgent;

public class ClientOSTests
{
    [Theory]
    [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36", "Windows", "10")]
    [InlineData("Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36", "Windows", "7")]
    [InlineData("Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36", "Windows", "8.1")]
    [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36", "Mac OS", "10_15_7")]
    [InlineData("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36", "Linux", "x86_64")]
    [InlineData("Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:89.0) Gecko/20100101 Firefox/89.0", "Ubuntu", "")]
    [InlineData("Mozilla/5.0 (iPhone; CPU iPhone OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.1 Mobile/15E148 Safari/604.1", "iOS", "14.6")]
    [InlineData("Mozilla/5.0 (iPad; CPU OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.1 Mobile/15E148 Safari/604.1", "iOS", "14.6")]
    [InlineData("Mozilla/5.0 (Linux; Android 11; SM-G996B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.72 Mobile Safari/537.36", "Android", "11")]
    public void Constructor_WithValidUserAgent_ShouldParseOSNameAndVersion(string userAgent, string expectedName, string expectedVersion)
    {
        // Act
        var os = new ClientOS(userAgent);

        // Assert
        Assert.Equal(expectedName, os.Name);
        Assert.Equal(expectedVersion, os.Version);
    }

    [Theory]
    [InlineData("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)", "Windows", "8")]
    [InlineData("Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)", "Windows", "7")]
    [InlineData("Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0)", "Windows", "Vista")]
    [InlineData("Mozilla/5.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0)", "Windows", "XP")]
    public void Constructor_WithWindowsUserAgent_ShouldParseVersionCorrectly(string userAgent, string expectedName, string expectedVersion)
    {
        // Act
        var os = new ClientOS(userAgent);

        // Assert
        Assert.Equal(expectedName, os.Name);
        Assert.Equal(expectedVersion, os.Version);
    }

    [Theory]
    [InlineData("Mozilla/5.0 (Windows Phone 10.0; Android 6.0.1; Microsoft; RM-1152) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Mobile Safari/537.36 Edge/15.15254", "Windows Phone", "10.0")]
    [InlineData("Mozilla/5.0 (Mobile; Windows Phone 8.1; Android 4.0; ARM; Trident/7.0; Touch; rv:11.0; IEMobile/11.0; NOKIA; Lumia 635) like iPhone OS 7_0_3 Mac OS X AppleWebKit/537 (KHTML, like Gecko) Mobile Safari/537", "Windows Phone", "8.1")]
    public void Constructor_WithWindowsPhoneUserAgent_ShouldParseCorrectly(string userAgent, string expectedName, string expectedVersion)
    {
        // Act
        var os = new ClientOS(userAgent);

        // Assert
        Assert.Equal(expectedName, os.Name);
        Assert.Equal(expectedVersion, os.Version);
    }

    [Theory]
    [InlineData("Mozilla/5.0 (BlackBerry; U; BlackBerry 9900; en) AppleWebKit/534.11+ (KHTML, like Gecko) Version/7.1.0.346 Mobile Safari/534.11+", "BlackBerry", "")]
    [InlineData("Mozilla/5.0 (BB10; Touch) AppleWebKit/537.10+ (KHTML, like Gecko) Version/10.0.9.2372 Mobile Safari/537.10+", "BlackBerry", "BB10")]
    public void Constructor_WithBlackBerryUserAgent_ShouldParseCorrectly(string userAgent, string expectedName, string expectedVersion)
    {
        // Act
        var os = new ClientOS(userAgent);

        // Assert
        Assert.Equal(expectedName, os.Name);
        Assert.Equal(expectedVersion, os.Version);
    }

    [Fact]
    public void Constructor_WithNullUserAgent_ShouldNotThrow()
    {
        // Arrange & Act & Assert
        var exception = Record.Exception(() => new ClientOS(null!));
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithEmptyUserAgent_ShouldNotThrow()
    {
        // Arrange & Act & Assert
        var exception = Record.Exception(() => new ClientOS(""));
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithInvalidUserAgent_ShouldNotThrow()
    {
        // Arrange & Act & Assert
        var exception = Record.Exception(() => new ClientOS("invalid-user-agent"));
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("invalid")]
    [InlineData("Mozilla/5.0")]
    [InlineData("complete-garbage-user-agent-string")]
    public void Constructor_WithEdgeCaseUserAgents_ShouldNotThrow(string userAgent)
    {
        // Act & Assert
        var exception = Record.Exception(() => new ClientOS(userAgent));
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithVeryLongUserAgent_ShouldNotThrow()
    {
        // Arrange
        var longUserAgent = new string('a', 10000);

        // Act & Assert
        var exception = Record.Exception(() => new ClientOS(longUserAgent));
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("Mozilla/5.0 (compatible; bingbot/2.0; +http://www.bing.com/bingbot.htm)")]
    [InlineData("Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)")]
    [InlineData("facebookexternalhit/1.1 (+http://www.facebook.com/externalhit_uatext.php)")]
    public void Constructor_WithBotUserAgents_ShouldNotThrow(string userAgent)
    {
        // Act
        var os = new ClientOS(userAgent);

        // Assert - Bots typically don't have OS info, so we just check it doesn't throw
        Assert.NotNull(os.Name);
        Assert.NotNull(os.Version);
    }

    [Theory]
    [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:89.0) Gecko/20100101 Firefox/89.0", "Mac OS", "10.15")]
    [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36", "Mac OS", "10_14_6")]
    [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.1.2 Safari/605.1.15", "Mac OS", "10_13_6")]
    public void Constructor_WithMacOSUserAgent_ShouldParseCorrectly(string userAgent, string expectedName, string expectedVersion)
    {
        // Act
        var os = new ClientOS(userAgent);

        // Assert
        Assert.Equal(expectedName, os.Name);
        Assert.Equal(expectedVersion, os.Version);
    }

    [Theory]
    [InlineData("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36", "Linux", "x86_64")]
    [InlineData("Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:89.0) Gecko/20100101 Firefox/89.0", "Ubuntu", "")]
    [InlineData("Mozilla/5.0 (X11; CrOS x86_64 13904.97.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.164 Safari/537.36", "CrOS", "x86_64")]
    public void Constructor_WithLinuxUserAgent_ShouldParseCorrectly(string userAgent, string expectedName, string expectedVersion)
    {
        // Act
        var os = new ClientOS(userAgent);

        // Assert
        Assert.Equal(expectedName, os.Name);
        Assert.Equal(expectedVersion, os.Version);
    }
} 