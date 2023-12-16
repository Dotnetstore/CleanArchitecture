using Application.Common.Services;
using Domain.Models;
using FluentAssertions;

namespace Application.Tests.Common.Services;

public class SwitchTests
{
    private record Person(string Name);

    [Fact]
    public void SwitchResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        Result<Person> resultPerson = new Person("Hans");
        void OnValueAction(Person person) => person.Should().BeEquivalentTo(resultPerson.Value);
        void OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        var action = () => resultPerson.Switch(
            OnValueAction,
            OnErrorsAction);

        action.Should().NotThrow();
    }

    [Fact]
    public void SwitchResult_WhenHasError_ShouldExecuteOnErrorAction()
    {
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        void OnValueAction(Person _) => throw new Exception("Should not be called");
        void OnErrorsAction(IReadOnlyList<Error> errors) => errors.Should().BeEquivalentTo(resultPerson.Errors);

        var action = () => resultPerson.Switch(
            OnValueAction,
            OnErrorsAction);

        action.Should().NotThrow();
    }

    [Fact]
    public void SwitchFirstResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        Result<Person> resultPerson = new Person("Hans");
        void OnValueAction(Person person) => person.Should().BeEquivalentTo(resultPerson.Value);
        void OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        var action = () => resultPerson.SwitchFirst(
            OnValueAction,
            OnFirstErrorAction);

        action.Should().NotThrow();
    }

    [Fact]
    public void SwitchFirstResult_WhenHasError_ShouldExecuteOnFirstErrorAction()
    {
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        void OnValueAction(Person _) => throw new Exception("Should not be called");
        void OnFirstErrorAction(Error errors)
            => errors.Should().BeEquivalentTo(resultPerson.Errors[0])
                .And.BeEquivalentTo(resultPerson.FirstError);

        var action = () => resultPerson.SwitchFirst(
            OnValueAction,
            OnFirstErrorAction);

        action.Should().NotThrow();
    }
}