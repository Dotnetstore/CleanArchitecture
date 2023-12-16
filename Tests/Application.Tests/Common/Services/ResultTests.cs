using Application.Common.Services;
using Domain.Enums;
using Domain.Models;
using FluentAssertions;

namespace Application.Tests.Common.Services;

public class ResultTests
{
    private record Person(string Name);

    [Fact]
    public void CreateFromFactory_WhenAccessingValue_ShouldReturnValue()
    {
        IEnumerable<string> value = new[] { "value" };

        var errorOrPerson = ErrorOrFactory.From(value);

        errorOrPerson.IsError.Should().BeFalse();
        errorOrPerson.Value.Should().BeSameAs(value);
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        IEnumerable<string> value = new[] { "value" };
        var errorOrPerson = ErrorOrFactory.From(value);

        var errors = errorOrPerson.Errors;

        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        IEnumerable<string> value = new[] { "value" };
        var errorOrPerson = ErrorOrFactory.From(value);

        var errors = errorOrPerson.ErrorsOrEmptyList;

        errors.Should().BeEmpty();
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        IEnumerable<string> value = new[] { "value" };
        var errorOrPerson = ErrorOrFactory.From(value);

        var firstError = errorOrPerson.FirstError;

        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingValue_ShouldReturnValue()
    {
        IEnumerable<string> value = new[] { "value" };

        var errorOrPerson = Result.From(value);

        errorOrPerson.IsError.Should().BeFalse();
        errorOrPerson.Value.Should().BeSameAs(value);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        IEnumerable<string> value = new[] { "value" };
        var errorOrPerson = Result.From(value);

        var errors = errorOrPerson.Errors;

        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        IEnumerable<string> value = new[] { "value" };
        var errorOrPerson = Result.From(value);

        var errors = errorOrPerson.ErrorsOrEmptyList;

        errors.Should().BeEmpty();
    }

    [Fact]
    public void CreateFromValue_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        IEnumerable<string> value = new[] { "value" };
        var errorOrPerson = Result.From(value);

        var firstError = errorOrPerson.FirstError;

        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        var errors = new List<Error> { Error.Validation("User.Name", "Name is too short") };
        var errorOrPerson = Result<Person>.From(errors);

        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.Errors.Should().ContainSingle().Which.Should().Be(errors.Single());
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingErrorsOrEmptyList_ShouldReturnErrorList()
    {
        var errors = new List<Error> { Error.Validation("User.Name", "Name is too short") };
        var errorOrPerson = Result<Person>.From(errors);

        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.ErrorsOrEmptyList.Should().ContainSingle().Which.Should().Be(errors.Single());
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingValue_ShouldReturnDefault()
    {
        var errors = new List<Error> { Error.Validation("User.Name", "Name is too short") };
        var errorOrPerson = Result<Person>.From(errors);

        var value = errorOrPerson.Value;

        value.Should().Be(default);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingResult_ShouldReturnValue()
    {
        var result = new Person("Hans");

        Result<Person> errorOr = result;

        errorOr.IsError.Should().BeFalse();
        errorOr.Value.Should().Be(result);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        Result<Person> errorOrPerson = new Person("Hans");

        var errors = errorOrPerson.Errors;

        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        Result<Person> errorOrPerson = new Person("Hans");

        var firstError = errorOrPerson.FirstError;

        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastPrimitiveResult_WhenAccessingResult_ShouldReturnValue()
    {
        const int result = 4;

        Result<int> errorOrInt = result;

        errorOrInt.IsError.Should().BeFalse();
        errorOrInt.Value.Should().Be(result);
    }

    [Fact]
    public void ImplicitCastErrorOrType_WhenAccessingResult_ShouldReturnValue()
    {
        Result<Success> errorOrSuccess = ResultType.Success;
        Result<Created> errorOrCreated = ResultType.Created;
        Result<Deleted> errorOrDeleted = ResultType.Deleted;
        Result<Updated> errorOrUpdated = ResultType.Updated;

        errorOrSuccess.IsError.Should().BeFalse();
        errorOrSuccess.Value.Should().Be(ResultType.Success);

        errorOrCreated.IsError.Should().BeFalse();
        errorOrCreated.Value.Should().Be(ResultType.Created);

        errorOrDeleted.IsError.Should().BeFalse();
        errorOrDeleted.Value.Should().Be(ResultType.Deleted);

        errorOrUpdated.IsError.Should().BeFalse();
        errorOrUpdated.Value.Should().Be(ResultType.Updated);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingErrors_ShouldReturnErrorList()
    {
        var error = Error.Validation("User.Name", "Name is too short");

        Result<Person> errorOrPerson = error;

        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void ImplicitCastError_WhenAccessingValue_ShouldReturnDefault()
    {
        Result<Person> errorOrPerson = Error.Validation("User.Name", "Name is too short");

        var value = errorOrPerson.Value;

        value.Should().Be(default);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingFirstError_ShouldReturnError()
    {
        var error = Error.Validation("User.Name", "Name is too short");

        Result<Person> errorOrPerson = error;

        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.FirstError.Should().Be(error);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        var errors = new List<Error>
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };

        Result<Person> errorOrPerson = errors;

        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.Errors.Should().HaveCount(errors.Count).And.BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingErrors_ShouldReturnErrorArray()
    {
        var errors = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };

        Result<Person> errorOrPerson = errors;

        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.Errors.Should().HaveCount(errors.Length).And.BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        var errors = new List<Error>
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };

        Result<Person> errorOrPerson = errors;

        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.FirstError.Should().Be(errors[0]);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        var errors = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };

        Result<Person> errorOrPerson = errors;

        errorOrPerson.IsError.Should().BeTrue();
        errorOrPerson.FirstError.Should().Be(errors[0]);
    }
}