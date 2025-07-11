using Xunit;
using akhr.ir.Models;

namespace ScissorLink.Tests.Models;

public class DtoTests
{
    [Fact]
    public void DtoShortLink_DefaultConstructor_ShouldInitializeProperties()
    {
        // Act
        var dto = new DtoShortLink();

        // Assert
        Assert.Equal(0, dto.ID);
        Assert.Equal(string.Empty, dto.Token);
        Assert.Equal(string.Empty, dto.OriginLink);
        Assert.False(dto.IsPublish);
    }

    [Fact]
    public void DtoShortLink_PropertyAssignment_ShouldWork()
    {
        // Arrange
        var dto = new DtoShortLink();

        // Act
        dto.ID = 123;
        dto.Token = "test-token";
        dto.OriginLink = "https://example.com";
        dto.IsPublish = true;

        // Assert
        Assert.Equal(123, dto.ID);
        Assert.Equal("test-token", dto.Token);
        Assert.Equal("https://example.com", dto.OriginLink);
        Assert.True(dto.IsPublish);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("test-token")]
    [InlineData("very-long-token-that-should-still-work")]
    [InlineData("token-with-special-chars-!@#$%^&*()")]
    [InlineData("üñîçødé-tøkéñ")]
    public void DtoShortLink_Token_ShouldHandleVariousValues(string token)
    {
        // Arrange
        var dto = new DtoShortLink();

        // Act
        dto.Token = token;

        // Assert
        Assert.Equal(token, dto.Token);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("https://example.com")]
    [InlineData("https://very-long-domain-name-that-should-still-work.com/with/very/long/path")]
    [InlineData("https://example.com/path?param=value&another=value")]
    [InlineData("https://example.com/üñîçødé-päth")]
    public void DtoShortLink_OriginLink_ShouldHandleVariousValues(string originLink)
    {
        // Arrange
        var dto = new DtoShortLink();

        // Act
        dto.OriginLink = originLink;

        // Assert
        Assert.Equal(originLink, dto.OriginLink);
    }

    [Fact]
    public void DtoShortLink_NullValues_ShouldBeHandled()
    {
        // Arrange
        var dto = new DtoShortLink();

        // Act & Assert
        Assert.NotNull(dto.Token);
        Assert.NotNull(dto.OriginLink);
        
        // Properties should never be null due to string.Empty initialization
        Assert.Equal(string.Empty, dto.Token);
        Assert.Equal(string.Empty, dto.OriginLink);
    }

    [Fact]
    public void DtoShortLinkDetail_DefaultConstructor_ShouldInitializeProperties()
    {
        // Act
        var dto = new DtoShortLinkDetail();

        // Assert
        Assert.Equal(0, dto.ID);
        Assert.Equal(0, dto.ShortLinkID);
        Assert.Null(dto.Country);
        Assert.Null(dto.OS);
        Assert.Null(dto.Browser);
    }

    [Fact]
    public void DtoShortLinkDetail_PropertyAssignment_ShouldWork()
    {
        // Arrange
        var dto = new DtoShortLinkDetail();

        // Act
        dto.ID = 123;
        dto.ShortLinkID = 456;
        dto.Country = "US";
        dto.OS = "Windows";
        dto.Browser = "Chrome";

        // Assert
        Assert.Equal(123, dto.ID);
        Assert.Equal(456, dto.ShortLinkID);
        Assert.Equal("US", dto.Country);
        Assert.Equal("Windows", dto.OS);
        Assert.Equal("Chrome", dto.Browser);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("US")]
    [InlineData("United States")]
    [InlineData("Üñîtéd Stätés")]
    public void DtoShortLinkDetail_Country_ShouldHandleVariousValues(string country)
    {
        // Arrange
        var dto = new DtoShortLinkDetail();

        // Act
        dto.Country = country;

        // Assert
        Assert.Equal(country, dto.Country);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Windows")]
    [InlineData("Windows 10")]
    [InlineData("Windows 10 Pro")]
    [InlineData("macOS")]
    [InlineData("Linux")]
    [InlineData("Android")]
    [InlineData("iOS")]
    [InlineData("Unknown OS")]
    public void DtoShortLinkDetail_OS_ShouldHandleVariousValues(string os)
    {
        // Arrange
        var dto = new DtoShortLinkDetail();

        // Act
        dto.OS = os;

        // Assert
        Assert.Equal(os, dto.OS);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Chrome")]
    [InlineData("Chrome/91.0.4472.124")]
    [InlineData("Firefox")]
    [InlineData("Safari")]
    [InlineData("Edge")]
    [InlineData("Internet Explorer")]
    [InlineData("Opera")]
    [InlineData("Unknown Browser")]
    public void DtoShortLinkDetail_Browser_ShouldHandleVariousValues(string browser)
    {
        // Arrange
        var dto = new DtoShortLinkDetail();

        // Act
        dto.Browser = browser;

        // Assert
        Assert.Equal(browser, dto.Browser);
    }

    [Fact]
    public void DtoShortLinkDetail_NullValues_ShouldBeHandled()
    {
        // Arrange
        var dto = new DtoShortLinkDetail();

        // Act
        dto.Country = null;
        dto.OS = null;
        dto.Browser = null;

        // Assert
        Assert.Null(dto.Country);
        Assert.Null(dto.OS);
        Assert.Null(dto.Browser);
    }

    [Fact]
    public void DtoResult_DefaultConstructor_ShouldInitializeProperties()
    {
        // Act
        var dto = new DtoResult();

        // Assert
        Assert.Null(dto.Result);
        Assert.Null(dto.Error);
    }

    [Fact]
    public void DtoResult_PropertyAssignment_ShouldWork()
    {
        // Arrange
        var dto = new DtoResult();
        var resultObject = new { Id = 1, Name = "Test" };

        // Act
        dto.Result = resultObject;
        dto.Error = "Test error";

        // Assert
        Assert.Equal(resultObject, dto.Result);
        Assert.Equal("Test error", dto.Error);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Simple error")]
    [InlineData("Very long error message that should still work correctly")]
    [InlineData("Error with special chars: !@#$%^&*()")]
    [InlineData("Üñîçødé érrør méssägé")]
    public void DtoResult_Error_ShouldHandleVariousValues(string error)
    {
        // Arrange
        var dto = new DtoResult();

        // Act
        dto.Error = error;

        // Assert
        Assert.Equal(error, dto.Error);
    }

    [Theory]
    [InlineData("string result")]
    [InlineData(123)]
    [InlineData(true)]
    public void DtoResult_Result_ShouldHandleVariousTypes(object result)
    {
        // Arrange
        var dto = new DtoResult();

        // Act
        dto.Result = result;

        // Assert
        Assert.Equal(result, dto.Result);
    }

    [Fact]
    public void DtoResult_WithComplexObject_ShouldWork()
    {
        // Arrange
        var dto = new DtoResult();
        var complexObject = new
        {
            Id = 1,
            Name = "Test",
            Items = new[] { "item1", "item2" },
            Nested = new { Value = "nested" }
        };

        // Act
        dto.Result = complexObject;

        // Assert
        Assert.Equal(complexObject, dto.Result);
    }

    [Fact]
    public void DtoResult_Result_WithNullValue_ShouldWork()
    {
        // Arrange
        var dto = new DtoResult();

        // Act
        dto.Result = null;

        // Assert
        Assert.Null(dto.Result);
    }

    [Fact]
    public void DtoResult_NullValues_ShouldBeHandled()
    {
        // Arrange
        var dto = new DtoResult();

        // Act
        dto.Result = null;
        dto.Error = null;

        // Assert
        Assert.Null(dto.Result);
        Assert.Null(dto.Error);
    }
} 