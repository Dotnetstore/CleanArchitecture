using Domain.Services;
using FluentAssertions;

namespace Domain.Tests.Services;

public class EnumerationTests
{
    public class SampleEnumeration : Enumeration<SampleEnumeration>
    {
        public static readonly SampleEnumeration First = new(Guid.NewGuid(), "First");
        public static readonly SampleEnumeration Second = new(Guid.NewGuid(), "Second");

        private SampleEnumeration(Guid value, string name) : base(value, name)
        {
        }
    }
    
    [Fact]
    public void FromValue_ReturnsCorrectEnumeration()
    {
        var valueToFind = SampleEnumeration.First.Value;

        var result = SampleEnumeration.FromValue(valueToFind);

        result.Should().Be(SampleEnumeration.First);
    }
    
    [Fact]
    public void FromValue_WithInvalidValue_ReturnsNull()
    {
        var invalidValue = Guid.NewGuid();

        var result = SampleEnumeration.FromValue(invalidValue);

        result.Should().BeNull();
    }
    
    [Fact]
    public void FromName_ReturnsCorrectEnumeration()
    {
        var nameToFind = SampleEnumeration.Second.Name;

        var result = SampleEnumeration.FromName(nameToFind);

        result.Should().Be(SampleEnumeration.Second);
    }
    
    [Fact]
    public void FromName_WithInvalidName_ReturnsNull()
    {
        const string invalidName = "NonExistent";

        var result = SampleEnumeration.FromName(invalidName);

        result.Should().BeNull();
    }
    
    [Fact]
    public void Equals_ReturnsTrueForEqualEnumerations()
    {
        var enumeration1 = SampleEnumeration.First;
        var enumeration2 = SampleEnumeration.First;

        var result = enumeration1.Equals(enumeration2);

        result.Should().BeTrue();
    }
    
    [Fact]
    public void Equals_ReturnsFalseForDifferentEnumerations()
    {
        var enumeration1 = SampleEnumeration.First;
        var enumeration2 = SampleEnumeration.Second;

        var result = enumeration1.Equals(enumeration2);

        result.Should().BeFalse();
    }
    
    [Fact]
    public void GetHashCode_ReturnsSameValueForEqualEnumerations()
    {
        var enumeration1 = SampleEnumeration.First;
        var enumeration2 = SampleEnumeration.First;

        var hashCode1 = enumeration1.GetHashCode();
        var hashCode2 = enumeration2.GetHashCode();

        hashCode1.Should().Be(hashCode2);
    }
    
    [Fact]
    public void GetHashCode_ReturnsDifferentValueForDifferentEnumerations()
    {
        var enumeration1 = SampleEnumeration.First;
        var enumeration2 = SampleEnumeration.Second;

        var hashCode1 = enumeration1.GetHashCode();
        var hashCode2 = enumeration2.GetHashCode();

        hashCode1.Should().NotBe(hashCode2);
    }
}