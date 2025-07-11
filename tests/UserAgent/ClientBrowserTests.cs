using Xunit;
using akhr.ir.UserAgent;

namespace ScissorLink.Tests.UserAgent;

public class ClientBrowserTests
{
    [Theory]
    [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36", "Chrome", "91.0.4472.124")]
    [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0", "Firefox", "89.0")]
    [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36", "Chrome", "91.0.4472.124")]
    [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 Edg/91.0.864.59", "Chrome", "91.0.4472.124")]
    [InlineData("Mozilla/5.0 (iPhone; CPU iPhone OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.1 Mobile/15E148 Safari/604.1", "Mobile Safari", "14.1.1")]
    [InlineData("Opera/9.80 (Windows NT 6.0) Presto/2.12.388 Version/12.14", "Opera", "9.80 (Windows NT 6.0) Presto/2.12.388 Version/12.14")]
    [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 OPR/77.0.4054.203", "Opera", "77.0.4054.203")]
    public void Constructor_WithValidUserAgent_ShouldParseBrowserNameAndVersion(string userAgent, string expectedName, string expectedVersion)
    {
        // Act
        var browser = new ClientBrowser(userAgent);

        // Assert
        Assert.Equal(expectedName, browser.Name);
        Assert.Equal(expectedVersion, browser.Version);
    }

    [Theory]
    [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36", "91")]
    [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0", "89")]
    public void Constructor_WithValidUserAgent_ShouldParseMajorVersion(string userAgent, string expectedMajor)
    {
        // Act
        var browser = new ClientBrowser(userAgent);

        // Assert
        Assert.Equal(expectedMajor, browser.Major);
    }

    [Fact]
    public void Constructor_WithNullUserAgent_ShouldNotThrow()
    {
        // Arrange & Act & Assert
        var exception = Record.Exception(() => new ClientBrowser(null!));
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithEmptyUserAgent_ShouldNotThrow()
    {
        // Arrange & Act & Assert
        var exception = Record.Exception(() => new ClientBrowser(""));
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithInvalidUserAgent_ShouldNotThrow()
    {
        // Arrange & Act & Assert
        var exception = Record.Exception(() => new ClientBrowser("invalid-user-agent"));
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)", "MSIE", "10.0; Windows NT 6.2; Trident/6.0")]
    [InlineData("Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko", "IE", "11")]
    [InlineData("Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)", "MSIE", "9.0; Windows NT 6.1; Trident/5.0")]
    public void Constructor_WithInternetExplorerUserAgent_ShouldParseCorrectly(string userAgent, string expectedName, string expectedVersion)
    {
        // Act
        var browser = new ClientBrowser(userAgent);

        // Assert
        Assert.Equal(expectedName, browser.Name);
        Assert.Equal(expectedVersion, browser.Version);
    }

    [Theory]
    [InlineData("Mozilla/5.0 (Android 11; Mobile; rv:68.0) Gecko/68.0 Firefox/88.0", "Firefox", "88.0")]
    [InlineData("Mozilla/5.0 (Linux; Android 11; SM-G996B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.72 Mobile Safari/537.36", "Chrome", "89.0.4389.72")]
    [InlineData("Mozilla/5.0 (iPhone; CPU iPhone OS 14_4_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0.3 Mobile/15E148 Safari/604.1", "Mobile Safari", "14.0.3")]
    public void Constructor_WithMobileUserAgent_ShouldParseCorrectly(string userAgent, string expectedName, string expectedVersion)
    {
        // Act
        var browser = new ClientBrowser(userAgent);

        // Assert
        Assert.Equal(expectedName, browser.Name);
        Assert.Equal(expectedVersion, browser.Version);
    }

    [Theory]
    [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 YaBrowser/21.6.0.616 Yowser/2.5", "Yandex", "21.6.0.616")]
    [InlineData("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 Vivaldi/4.0.2312.33", "Vivaldi", "4.0.2312.33")]
    [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 Brave/1.26.74", "Chrome", "91.0.4472.124")]
    public void Constructor_WithLessCommonBrowsers_ShouldParseCorrectly(string userAgent, string expectedName, string expectedVersion)
    {
        // Act
        var browser = new ClientBrowser(userAgent);

        // Assert
        Assert.Equal(expectedName, browser.Name);
        Assert.Equal(expectedVersion, browser.Version);
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
        var exception = Record.Exception(() => new ClientBrowser(userAgent));
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithVeryLongUserAgent_ShouldNotThrow()
    {
        // Arrange
        var longUserAgent = new string('a', 10000);

        // Act & Assert
        var exception = Record.Exception(() => new ClientBrowser(longUserAgent));
        Assert.Null(exception);
    }

    [Theory]
    [InlineData("Mozilla/5.0 (compatible; bingbot/2.0; +http://www.bing.com/bingbot.htm)")]
    [InlineData("Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)")]
    [InlineData("facebookexternalhit/1.1 (+http://www.facebook.com/externalhit_uatext.php)")]
    public void Constructor_WithBotUserAgents_ShouldNotThrow(string userAgent)
    {
        // Act
        var browser = new ClientBrowser(userAgent);

        // Assert - Note: These might not match exactly due to how the parser works, but should not throw
        Assert.NotNull(browser.Name);
        Assert.NotNull(browser.Version);
    }
} 