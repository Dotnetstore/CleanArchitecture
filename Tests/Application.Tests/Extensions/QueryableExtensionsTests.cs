using System.Linq.Expressions;
using Application.Extensions;
using FluentAssertions.Execution;

namespace Application.Tests.Extensions;

public class QueryableExtensionsTests
{
    private class SampleEntity
    {
        public int Id { get; set; }
        public string? Name { get; init; }
    }
    
    [Fact]
    public void WhereNullable_WithNonNullParameter_ReturnsFilteredQueryable()
    {
        var data = new List<SampleEntity>
        {
            new() { Id = 1, Name = "John" },
            new() { Id = 2, Name = "Jane" },
        }.AsQueryable();

        const string parameter = "John"; // Non-null parameter

        var result = data.WhereNullable(parameter, e => e.Name == parameter);

        using (new AssertionScope())
        {
            result.Count().Should().Be(1);
            result.First().Name.Should().Be("John");
        }
    }
    
    [Fact]
    public void WhereNullable_WithNullParameter_ReturnsOriginalQueryable()
    {
        var data = new List<SampleEntity>
        {
            new() { Id = 1, Name = "John" },
            new() { Id = 2, Name = "Jane" },
        }.AsQueryable();

        string? parameter = null; // Null parameter

        var result = data.WhereNullable(parameter, e => e.Name == parameter);

        result.Count().Should().Be(2);
    }
    
    [Fact]
    public void WhereNullable_WithNullPredicate_ThrowsArgumentNullException()
    {
        var data = new List<SampleEntity>
        {
            new() { Id = 1, Name = "John" },
            new() { Id = 2, Name = "Jane" },
        }.AsQueryable();

        const string parameter = "John";
        Expression<Func<SampleEntity, bool>> predicate = null!; // Null predicate
        
        Assert.Throws<ArgumentNullException>(() => data.WhereNullable(parameter, predicate));
    }
}