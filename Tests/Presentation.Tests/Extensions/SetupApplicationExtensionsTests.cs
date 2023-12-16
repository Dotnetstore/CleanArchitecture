using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Presentation.Extensions;
using Presentation.Factories;
using Serilog;

namespace Presentation.Tests.Extensions;

public class SetupApplicationExtensionsTests
{
    private readonly WebApplicationBuilder _builder = WebApplication.CreateBuilder();
    private readonly ILogger _logger;

    public SetupApplicationExtensionsTests()
    {
        _logger = _builder.CreateLogger();
    }

    [Fact]
    public void AddServices_should_return_WebApplicationBuilder()
    {
        var result = _builder.AddServices(_logger);

        result.Should().BeOfType<WebApplicationBuilder>();
    }

    [Fact]
    public void AddLogger_should_return_WebApplicationBuilder()
    {
        var result = _builder
            .AddServices(_logger)
            .AddLogger(_logger);

        result.Should().BeOfType<WebApplicationBuilder>();
    }

    [Fact]
    public void BuildApplication_should_return_WebApplication()
    {
        var result = _builder
            .AddServices(_logger)
            .AddLogger(_logger)
            .BuildApplication();

        result.Should().BeOfType<WebApplication>();
    }

    [Fact]
    public void LoadDb_should_return_WebApplication()
    {
        var result = _builder
            .AddServices(_logger)
            .AddLogger(_logger)
            .BuildApplication()
            .LoadDb();

        result.Should().BeOfType<WebApplication>();
    }

    [Fact]
    public void CheckIfDatabaseIsUpdated_should_return_WebApplication()
    {
        var result = _builder
            .AddServices(_logger)
            .AddLogger(_logger)
            .BuildApplication()
            .LoadDb()
            .CheckIfDatabaseIsUpdated();

        result.Should().BeOfType<WebApplication>();
    }

    [Fact]
    public void AddSwagger_should_return_WebApplication()
    {
        var result = _builder
            .AddServices(_logger)
            .AddLogger(_logger)
            .BuildApplication()
            .LoadDb()
            .CheckIfDatabaseIsUpdated()
            .AddSwagger();

        result.Should().BeOfType<WebApplication>();
    }

    [Fact]
    public void AddMiddleware_should_return_WebApplication()
    {
        var result = _builder
            .AddServices(_logger)
            .AddLogger(_logger)
            .BuildApplication()
            .LoadDb()
            .CheckIfDatabaseIsUpdated()
            .AddSwagger()
            .AddMiddleware();

        result.Should().BeOfType<WebApplication>();
    }

    [Fact]
    public void AddApplicationServices_should_return_WebApplication()
    {
        var result = _builder
            .AddServices(_logger)
            .AddLogger(_logger)
            .BuildApplication()
            .LoadDb()
            .CheckIfDatabaseIsUpdated()
            .AddSwagger()
            .AddMiddleware()
            .AddApplicationServices();

        result.Should().BeOfType<WebApplication>();
    }

    [Fact]
    public void RunAppAsync_should_return_WebApplication()
    {
        var result = _builder
            .AddServices(_logger)
            .AddLogger(_logger)
            .BuildApplication()
            .LoadDb()
            .CheckIfDatabaseIsUpdated()
            .AddSwagger()
            .AddMiddleware()
            .AddApplicationServices()
            .RunAppAsync();

        result.Should().NotBeNull();
    }
}