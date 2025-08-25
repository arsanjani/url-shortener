using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;
using ScissorLink.Controllers;

namespace ScissorLink.Tests.Controllers;

public class ErrorControllerTests
{
    private readonly ErrorController _controller;
    private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;

    public ErrorControllerTests()
    {
        _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

        // Setup mock to return a fake web root path
        var webRootPath = Path.Combine(Path.GetTempPath(), "test_wwwroot");
        _mockWebHostEnvironment.Setup(x => x.WebRootPath).Returns(webRootPath);

        _controller = new ErrorController(_mockWebHostEnvironment.Object);

        // Setup HttpContext for the controller
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public void Error_WithValidCode_WhenIndexHtmlExists_ShouldReturnPhysicalFile()
    {
        // Arrange
        var errorCode = 404;
        var webRootPath = _mockWebHostEnvironment.Object.WebRootPath;
        var indexPath = Path.Combine(webRootPath, "index.html");

        // Create the directory and file for testing
        Directory.CreateDirectory(webRootPath);
        File.WriteAllText(indexPath, "<html><body>Test</body></html>");

        // Act
        var result = _controller.Error(errorCode);

        // Assert
        Assert.IsType<PhysicalFileResult>(result);
        var physicalFileResult = (PhysicalFileResult)result;
        Assert.Equal(indexPath, physicalFileResult.FileName);
        Assert.Equal("text/html", physicalFileResult.ContentType);

        // Verify status code is set
        Assert.Equal(errorCode, _controller.HttpContext.Response.StatusCode);

        // Cleanup
        File.Delete(indexPath);
        Directory.Delete(webRootPath);
    }

    [Fact]
    public void Error_WithValidCode_WhenIndexHtmlDoesNotExist_ShouldReturnContentResult()
    {
        // Arrange
        var errorCode = 404;

        // Act
        var result = _controller.Error(errorCode);

        // Assert
        Assert.IsType<ContentResult>(result);
        var contentResult = (ContentResult)result;
        Assert.Equal($"Error {errorCode}: The page you requested could not be found.", contentResult.Content);
        Assert.Equal("text/plain", contentResult.ContentType);

        // Verify status code is set
        Assert.Equal(errorCode, _controller.HttpContext.Response.StatusCode);
    }

    [Theory]
    [InlineData(400)]
    [InlineData(401)]
    [InlineData(403)]
    [InlineData(404)]
    [InlineData(500)]
    [InlineData(503)]
    public void Error_WithVariousErrorCodes_ShouldReturnContentResult(int errorCode)
    {
        // Act
        var result = _controller.Error(errorCode);

        // Assert
        Assert.IsType<ContentResult>(result);
        var contentResult = (ContentResult)result;
        Assert.Equal($"Error {errorCode}: The page you requested could not be found.", contentResult.Content);
        Assert.Equal("text/plain", contentResult.ContentType);

        // Verify status code is set
        Assert.Equal(errorCode, _controller.HttpContext.Response.StatusCode);
    }

    [Fact]
    public void Error_ShouldSetStatusCode()
    {
        // Arrange
        var errorCode = 404;

        // Act
        var result = _controller.Error(errorCode);

        // Assert
        // The controller no longer sets ViewBag properties, but it should set the response status code
        Assert.Equal(errorCode, _controller.HttpContext.Response.StatusCode);

        // Verify the result type based on whether index.html exists
        Assert.IsType<ContentResult>(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Error_WithEdgeCaseCodes_ShouldReturnContentResult(int errorCode)
    {
        // Act
        var result = _controller.Error(errorCode);

        // Assert
        Assert.IsType<ContentResult>(result);
        var contentResult = (ContentResult)result;
        Assert.Equal($"Error {errorCode}: The page you requested could not be found.", contentResult.Content);
        Assert.Equal("text/plain", contentResult.ContentType);

        // Verify status code is set
        Assert.Equal(errorCode, _controller.HttpContext.Response.StatusCode);
    }
} 