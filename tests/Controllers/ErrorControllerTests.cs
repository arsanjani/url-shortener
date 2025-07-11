using Xunit;
using Microsoft.AspNetCore.Mvc;
using akhr.ir.Controllers;

namespace ScissorLink.Tests.Controllers;

public class ErrorControllerTests
{
    private readonly ErrorController _controller;

    public ErrorControllerTests()
    {
        _controller = new ErrorController();
    }

    [Fact]
    public void Error_WithValidCode_ShouldReturnView()
    {
        // Arrange
        var errorCode = 404;

        // Act
        var result = _controller.Error(errorCode);

        // Assert
        Assert.IsType<ViewResult>(result);
        var viewResult = (ViewResult)result;
        Assert.Equal("Views/404.cshtml", viewResult.ViewName);
    }

    [Theory]
    [InlineData(400)]
    [InlineData(401)]
    [InlineData(403)]
    [InlineData(404)]
    [InlineData(500)]
    [InlineData(503)]
    public void Error_WithVariousErrorCodes_ShouldReturnView(int errorCode)
    {
        // Act
        var result = _controller.Error(errorCode);

        // Assert
        Assert.IsType<ViewResult>(result);
        var viewResult = (ViewResult)result;
        Assert.Equal("Views/404.cshtml", viewResult.ViewName);
    }

    [Fact]
    public void Error_ShouldSetViewBagProperties()
    {
        // Arrange
        var errorCode = 404;

        // Act
        var result = _controller.Error(errorCode);

        // Assert
        Assert.IsType<ViewResult>(result);
        var viewResult = (ViewResult)result;
        
        Assert.Equal("Oops! 404", viewResult.ViewData["PageTitle"]);
        Assert.Equal("!این راه به جایی نمیرسد", viewResult.ViewData["Title"]);
        Assert.Equal("شرمنده، به نظر می‌رسد مشکلی پدید آمده باشد. صفحه درخواستی پیدا نشد که نشد", viewResult.ViewData["SubTitle"]);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Error_WithEdgeCaseCodes_ShouldReturnView(int errorCode)
    {
        // Act
        var result = _controller.Error(errorCode);

        // Assert
        Assert.IsType<ViewResult>(result);
        var viewResult = (ViewResult)result;
        Assert.Equal("Views/404.cshtml", viewResult.ViewName);
    }
} 