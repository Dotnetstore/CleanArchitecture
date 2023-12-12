namespace Application.Common.Services;

/// <summary>
/// Provides factory methods for creating instances of <see cref="Result{TValue}"/>.
/// </summary>
public static class ErrorOrFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="Result{TValue}"/> with a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>An instance of <see cref="Result{TValue}"/> containing the provided value.</returns>
    public static Result<TValue> From<TValue>(TValue value)
    {
        return value;
    }
}