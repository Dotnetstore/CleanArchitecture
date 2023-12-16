using Application.Common.Services;
using Domain.Models;
using FluentAssertions;

namespace Application.Tests.Common.Services;

public class MatchAsyncTests
{
    private record Person(string Name);

    [Fact]
    public async Task MatchAsyncErrorOr_WhenHasValue_ShouldExecuteOnValueAction()
    {
        Result<Person> resultPerson = new Person("Hans");
        Task<string> OnValueAction(Person person)
        {
            person.Should().BeEquivalentTo(resultPerson.Value);
            return Task.FromResult("Nice");
        }

        Task<string> OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        var action = async () => await resultPerson.MatchAsync(
            OnValueAction,
            OnErrorsAction);

        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }

    [Fact]
    public async Task MatchAsyncErrorOr_WhenHasError_ShouldExecuteOnErrorAction()
    {
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task<string> OnValueAction(Person _) => throw new Exception("Should not be called");

        Task<string> OnErrorsAction(IReadOnlyList<Error> errors)
        {
            errors.Should().BeEquivalentTo(resultPerson.Errors);
            return Task.FromResult("Nice");
        }

        var action = async () => await resultPerson.MatchAsync(
            OnValueAction,
            OnErrorsAction);

        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }

    [Fact]
    public async Task MatchFirstAsyncErrorOr_WhenHasValue_ShouldExecuteOnValueAction()
    {
        Result<Person> resultPerson = new Person("Hans");
        Task<string> OnValueAction(Person person)
        {
            person.Should().BeEquivalentTo(resultPerson.Value);
            return Task.FromResult("Nice");
        }

        Task<string> OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        var action = async () => await resultPerson.MatchFirstAsync(
            OnValueAction,
            OnFirstErrorAction);

        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }

    [Fact]
    public async Task MatchFirstAsyncErrorOr_WhenHasError_ShouldExecuteOnFirstErrorAction()
    {
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task<string> OnValueAction(Person _) => throw new Exception("Should not be called");
        Task<string> OnFirstErrorAction(Error errors)
        {
            errors.Should().BeEquivalentTo(resultPerson.Errors[0])
                .And.BeEquivalentTo(resultPerson.FirstError);

            return Task.FromResult("Nice");
        }

        var action = async () => await resultPerson.MatchFirstAsync(
            OnValueAction,
            OnFirstErrorAction);

        (await action.Should().NotThrowAsync()).Subject.Should().Be("Nice");
    }
}