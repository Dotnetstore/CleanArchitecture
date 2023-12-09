using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Presentation.Extensions;

namespace Presentation.Tests.Extensions;

public class SetupApplicationExtensionsTests
{
    private WebApplicationBuilder _builder;

    public SetupApplicationExtensionsTests()
    {
        _builder = WebApplication.CreateBuilder();
    }
    
    [Fact]
    public void AddServices_should_return_WebApplicationBuilder()
    {
        _builder.AddServices().Should().BeOfType<WebApplicationBuilder>();
    }

    [Fact]
    public void Build_should_return_WebApplication()
    {
        _builder
            .AddServices()
            .BuildApplication()
            .Should()
            .BeOfType<WebApplication>();
    }

    [Fact]
    public void AddSwagger_should_return_WebApplication()
    {
        _builder
            .AddServices()
            .BuildApplication()
            .AddSwagger()
            .Should()
            .BeOfType<WebApplication>();
    }

    [Fact]
    public void AddApplicationServices_should_return_WebApplication()
    {
        _builder
            .AddServices()
            .BuildApplication()
            .AddSwagger()
            .AddApplicationServices()
            .Should()
            .BeOfType<WebApplication>();
    }

    [Fact]
    public void RunAppAsync_should_NotBeNull()
    {
        _builder
            .AddServices()
            .BuildApplication()
            .AddSwagger()
            .AddApplicationServices()
            .RunAppAsync()
            .Should()
            .NotBeNull();
    }
}