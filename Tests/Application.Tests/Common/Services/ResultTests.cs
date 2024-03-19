using Domain.Enums;

namespace Application.Tests.Common.Services;

public class ResultTests
{
    private record Person(string Name);

    [Fact]
    public void CreateFromFactory_WhenAccessingValue_ShouldReturnValue()
    {
        IEnumerable<string> value = new[] { "value" };

        var resultPerson = ErrorOrFactory.From(value);

        resultPerson.IsError.Should().BeFalse();
        resultPerson.Value.Should().BeSameAs(value);
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        IEnumerable<string> value = new[] { "value" };
        var resultPerson = ErrorOrFactory.From(value);

        var errors = resultPerson.Errors;

        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        IEnumerable<string> value = new[] { "value" };
        var resultPerson = ErrorOrFactory.From(value);

        var errors = resultPerson.ErrorsOrEmptyList;

        errors.Should().BeEmpty();
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        IEnumerable<string> value = new[] { "value" };
        var resultPerson = ErrorOrFactory.From(value);

        var firstError = resultPerson.FirstError;

        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingValue_ShouldReturnValue()
    {
        IEnumerable<string> value = new[] { "value" };

        var resultPerson = Result.From(value);

        resultPerson.IsError.Should().BeFalse();
        resultPerson.Value.Should().BeSameAs(value);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        IEnumerable<string> value = new[] { "value" };
        var resultPerson = Result.From(value);

        var errors = resultPerson.Errors;

        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrorsOrEmptyList_ShouldReturnEmptyList()
    {
        IEnumerable<string> value = new[] { "value" };
        var resultPerson = Result.From(value);

        var errors = resultPerson.ErrorsOrEmptyList;

        errors.Should().BeEmpty();
    }

    [Fact]
    public void CreateFromValue_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        IEnumerable<string> value = new[] { "value" };
        var resultPerson = Result.From(value);

        var firstError = resultPerson.FirstError;

        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        var errors = new List<Error> { Error.Validation("User.Name", "Name is too short") };
        var resultPerson = Result<Person>.From(errors);

        resultPerson.IsError.Should().BeTrue();
        resultPerson.Errors.Should().ContainSingle().Which.Should().Be(errors.Single());
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingErrorsOrEmptyList_ShouldReturnErrorList()
    {
        var errors = new List<Error> { Error.Validation("User.Name", "Name is too short") };
        var resultPerson = Result<Person>.From(errors);

        resultPerson.IsError.Should().BeTrue();
        resultPerson.ErrorsOrEmptyList.Should().ContainSingle().Which.Should().Be(errors.Single());
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingValue_ShouldReturnDefault()
    {
        var errors = new List<Error> { Error.Validation("User.Name", "Name is too short") };
        var resultPerson = Result<Person>.From(errors);

        var value = resultPerson.Value;

        value.Should().Be(default);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingResult_ShouldReturnValue()
    {
        var resultPerson = new Person("Hans");

        Result<Person> result = resultPerson;

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(resultPerson);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingErrors_ShouldReturnUnexpectedError()
    {
        Result<Person> resultPerson = new Person("Hans");

        var errors = resultPerson.Errors;

        errors.Should().ContainSingle().Which.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingFirstError_ShouldReturnUnexpectedError()
    {
        Result<Person> resultPerson = new Person("Hans");

        var firstError = resultPerson.FirstError;

        firstError.Type.Should().Be(ErrorType.Unexpected);
    }

    [Fact]
    public void ImplicitCastPrimitiveResult_WhenAccessingResult_ShouldReturnValue()
    {
        const int result = 4;

        Result<int> resultInt = result;

        resultInt.IsError.Should().BeFalse();
        resultInt.Value.Should().Be(result);
    }

    [Fact]
    public void ImplicitCastErrorOrType_WhenAccessingResult_ShouldReturnValue()
    {
        Result<Success> resultSuccess = ResultType.Success;
        Result<Created> resultCreated = ResultType.Created;
        Result<Deleted> resultDeleted = ResultType.Deleted;
        Result<Updated> resultUpdated = ResultType.Updated;

        resultSuccess.IsError.Should().BeFalse();
        resultSuccess.Value.Should().Be(ResultType.Success);

        resultCreated.IsError.Should().BeFalse();
        resultCreated.Value.Should().Be(ResultType.Created);

        resultDeleted.IsError.Should().BeFalse();
        resultDeleted.Value.Should().Be(ResultType.Deleted);

        resultUpdated.IsError.Should().BeFalse();
        resultUpdated.Value.Should().Be(ResultType.Updated);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingErrors_ShouldReturnErrorList()
    {
        var error = Error.Validation("User.Name", "Name is too short");

        Result<Person> resultPerson = error;

        resultPerson.IsError.Should().BeTrue();
        resultPerson.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void ImplicitCastError_WhenAccessingValue_ShouldReturnDefault()
    {
        Result<Person> resultPerson = Error.Validation("User.Name", "Name is too short");

        var value = resultPerson.Value;

        value.Should().Be(default);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingFirstError_ShouldReturnError()
    {
        var error = Error.Validation("User.Name", "Name is too short");

        Result<Person> resultPerson = error;

        resultPerson.IsError.Should().BeTrue();
        resultPerson.FirstError.Should().Be(error);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        var errors = new List<Error>
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };

        Result<Person> resultPerson = errors;

        resultPerson.IsError.Should().BeTrue();
        resultPerson.Errors.Should().HaveCount(errors.Count).And.BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingErrors_ShouldReturnErrorArray()
    {
        var errors = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };

        Result<Person> resultPerson = errors;

        resultPerson.IsError.Should().BeTrue();
        resultPerson.Errors.Should().HaveCount(errors.Length).And.BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        var errors = new List<Error>
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };

        Result<Person> resultPerson = errors;

        resultPerson.IsError.Should().BeTrue();
        resultPerson.FirstError.Should().Be(errors[0]);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        var errors = new[]
        {
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        };

        Result<Person> resultPerson = errors;

        resultPerson.IsError.Should().BeTrue();
        resultPerson.FirstError.Should().Be(errors[0]);
    }
}