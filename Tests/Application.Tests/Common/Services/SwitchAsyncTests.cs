namespace Application.Tests.Common.Services;

public class SwitchAsyncTests
{
    private record Person(string Name);

    [Fact]
    public async Task SwitchAsyncResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        Result<Person> resultPerson = new Person("Hans");
        Task OnValueAction(Person person) => Task.FromResult(person.Should().BeEquivalentTo(resultPerson.Value));
        Task OnErrorsAction(IReadOnlyList<Error> _) => throw new Exception("Should not be called");

        var action = async () => await resultPerson.SwitchAsync(
            OnValueAction,
            OnErrorsAction);

        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task SwitchAsyncResult_WhenHasError_ShouldExecuteOnErrorAction()
    {
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task OnValueAction(Person _) => throw new Exception("Should not be called");
        Task OnErrorsAction(IReadOnlyList<Error> errors) => Task.FromResult(errors.Should().BeEquivalentTo(resultPerson.Errors));

        var action = async () => await resultPerson.SwitchAsync(
            OnValueAction,
            OnErrorsAction);

        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task SwitchAsyncFirstResult_WhenHasValue_ShouldExecuteOnValueAction()
    {
        Result<Person> resultPerson = new Person("Hans");
        Task OnValueAction(Person person) => Task.FromResult(person.Should().BeEquivalentTo(resultPerson.Value));
        Task OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        var action = async () => await resultPerson.SwitchFirstAsync(
            OnValueAction,
            OnFirstErrorAction);

        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task SwitchFirstAsyncResult_WhenHasError_ShouldExecuteOnFirstErrorAction()
    {
        Result<Person> resultPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        Task OnValueAction(Person _) => throw new Exception("Should not be called");
        Task OnFirstErrorAction(Error errors)
            => Task.FromResult(errors.Should().BeEquivalentTo(resultPerson.Errors[0])
                .And.BeEquivalentTo(resultPerson.FirstError));

        var action = async () => await resultPerson.SwitchFirstAsync(
            OnValueAction,
            OnFirstErrorAction);

        await action.Should().NotThrowAsync();
    }
}