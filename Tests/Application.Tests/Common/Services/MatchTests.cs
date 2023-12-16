namespace Application.Tests.Common.Services;

public class MatchTests
{
    private record Person(string Name);

    [Fact]
    public void MatchResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        Result<Person> resultPerson = new Person("Hans");
        string OnValueAction(Person person)
        {
            person.Should().BeEquivalentTo(resultPerson.Value);
            return "Nice";
        }

        string OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        Func<string> action = () => resultPerson.Match(
            OnValueAction,
            OnErrorsAction);

        action.Should().NotThrow().Subject.Should().Be("Nice");
    }

    [Fact]
    public void MatchResult_WhenHasError_ShouldExecuteOnErrorAction()
    {
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        string OnValueAction(Person _) => throw new Exception("Should not be called");

        string OnErrorsAction(IReadOnlyList<Error> errors)
        {
            errors.Should().BeEquivalentTo(resultPerson.Errors);
            return "Nice";
        }

        Func<string> action = () => resultPerson.Match(
            OnValueAction,
            OnErrorsAction);

        action.Should().NotThrow().Subject.Should().Be("Nice");
    }

    [Fact]
    public void MatchFirstResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        Result<Person> resultPerson = new Person("Hans");
        string OnValueAction(Person person)
        {
            person.Should().BeEquivalentTo(resultPerson.Value);
            return "Nice";
        }

        string OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        var action = () => resultPerson.MatchFirst(
            OnValueAction,
            OnFirstErrorAction);

        action.Should().NotThrow().Subject.Should().Be("Nice");
    }

    [Fact]
    public void MatchFirstResult_WhenHasError_ShouldExecuteOnFirstErrorAction()
    {
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        string OnValueAction(Person _) => throw new Exception("Should not be called");
        string OnFirstErrorAction(Error errors)
        {
            errors.Should().BeEquivalentTo(resultPerson.Errors[0])
                .And.BeEquivalentTo(resultPerson.FirstError);

            return "Nice";
        }

        var action = () => resultPerson.MatchFirst(
            OnValueAction,
            OnFirstErrorAction);

        action.Should().NotThrow().Subject.Should().Be("Nice");
    }
}