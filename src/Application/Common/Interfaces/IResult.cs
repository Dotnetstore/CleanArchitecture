using Application.Common.Services;

namespace Application.Common.Interfaces;

public interface IResult
{
    /// <summary>
    /// Gets the list of errors.
    /// </summary>
    List<Error>? Errors { get; }

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    bool IsError { get; }
}