using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using akhr.ir.Common;
using System.Text.Json;
using System.IO;
using System.Text;

namespace ScissorLink.Tests.Common;

public class ErrorHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _mockLogger;
    private readonly ErrorHandlingMiddleware _middleware;
    private readonly Mock<RequestDelegate> _mockNext;

    public ErrorHandlingMiddlewareTests()
    {
        _mockLogger = new Mock<ILogger<ErrorHandlingMiddleware>>();
        _mockNext = new Mock<RequestDelegate>();
        _middleware = new ErrorHandlingMiddleware(_mockNext.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task InvokeAsync_WithNoException_ShouldCallNext()
    {
        // Arrange
        var context = new DefaultHttpContext();
        _mockNext.Setup(x => x(It.IsAny<HttpContext>()))
                 .Returns(Task.CompletedTask);

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        _mockNext.Verify(x => x(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WithException_ShouldHandleException()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        
        var exception = new Exception("Test exception");
        _mockNext.Setup(x => x(It.IsAny<HttpContext>()))
                 .ThrowsAsync(exception);

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        _mockNext.Verify(x => x(context), Times.Once);
        
        // Check that the response was set correctly
        Assert.Equal(500, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);
        
        // Check that the logger was called
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WithException_ShouldReturnJsonError()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var responseStream = new MemoryStream();
        context.Response.Body = responseStream;
        
        var exception = new Exception("Test exception message");
        _mockNext.Setup(x => x(It.IsAny<HttpContext>()))
                 .ThrowsAsync(exception);

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        responseStream.Position = 0;
        var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
        
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var errorResponse = JsonSerializer.Deserialize<JsonElement>(responseBody, options);
        
        Assert.False(errorResponse.GetProperty("result").GetBoolean());
        Assert.Equal("Test exception message", errorResponse.GetProperty("error").GetString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("Short error")]
    [InlineData("Very long error message that should still be handled correctly by the middleware")]
    [InlineData("Error with special characters: !@#$%^&*()")]
    [InlineData("Üñîçødé érrør méssägé")]
    public async Task InvokeAsync_WithVariousExceptionMessages_ShouldHandleCorrectly(string errorMessage)
    {
        // Arrange
        var context = new DefaultHttpContext();
        var responseStream = new MemoryStream();
        context.Response.Body = responseStream;
        
        var exception = new Exception(errorMessage);
        _mockNext.Setup(x => x(It.IsAny<HttpContext>()))
                 .ThrowsAsync(exception);

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        responseStream.Position = 0;
        var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
        
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var errorResponse = JsonSerializer.Deserialize<JsonElement>(responseBody, options);
        
        Assert.False(errorResponse.GetProperty("result").GetBoolean());
        Assert.Equal(errorMessage, errorResponse.GetProperty("error").GetString());
    }

    [Fact]
    public async Task InvokeAsync_WithNullException_ShouldHandleGracefully()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var responseStream = new MemoryStream();
        context.Response.Body = responseStream;
        
        var exception = new Exception(null);
        _mockNext.Setup(x => x(It.IsAny<HttpContext>()))
                 .ThrowsAsync(exception);

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        responseStream.Position = 0;
        var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
        
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var errorResponse = JsonSerializer.Deserialize<JsonElement>(responseBody, options);
        
        Assert.False(errorResponse.GetProperty("result").GetBoolean());
        // Exception with null message should still be handled
        Assert.NotNull(errorResponse.GetProperty("error").GetString());
    }

    [Fact]
    public async Task InvokeAsync_WithNestedExceptions_ShouldHandleCorrectly()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var responseStream = new MemoryStream();
        context.Response.Body = responseStream;
        
        var innerException = new InvalidOperationException("Inner exception");
        var outerException = new Exception("Outer exception", innerException);
        _mockNext.Setup(x => x(It.IsAny<HttpContext>()))
                 .ThrowsAsync(outerException);

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        responseStream.Position = 0;
        var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
        
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var errorResponse = JsonSerializer.Deserialize<JsonElement>(responseBody, options);
        
        Assert.False(errorResponse.GetProperty("result").GetBoolean());
        Assert.Equal("Outer exception", errorResponse.GetProperty("error").GetString());
    }
} 