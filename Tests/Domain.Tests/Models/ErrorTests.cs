using Domain.Enums;
using Domain.Models;
using FluentAssertions;

namespace Domain.Tests.Models;

public class ErrorTests
{
    private const string ErrorCode = "ErrorCode";
    private const string ErrorDescription = "ErrorDescription";

    [Fact]
    public void CreateError_WhenFailureError_ShouldHaveErrorTypeFailure()
    {
        var error = Error.Failure(ErrorCode, ErrorDescription);

        ValidateError(error, expectedErrorType: ErrorType.Failure);
    }

    [Fact]
    public void CreateError_WhenUnexpectedError_ShouldHaveErrorTypeFailure()
    {
        var error = Error.Unexpected(ErrorCode, ErrorDescription);

        ValidateError(error, expectedErrorType: ErrorType.Unexpected);
    }

    [Fact]
    public void CreateError_WhenValidationError_ShouldHaveErrorTypeValidation()
    {
        var error = Error.Validation(ErrorCode, ErrorDescription);

        ValidateError(error, expectedErrorType: ErrorType.Validation);
    }

    [Fact]
    public void CreateError_WhenConflictError_ShouldHaveErrorTypeConflict()
    {
        var error = Error.Conflict(ErrorCode, ErrorDescription);

        ValidateError(error, expectedErrorType: ErrorType.Conflict);
    }

    [Fact]
    public void CreateError_WhenNotFoundError_ShouldHaveErrorTypeNotFound()
    {
        var error = Error.NotFound(ErrorCode, ErrorDescription);

        ValidateError(error, expectedErrorType: ErrorType.NotFound);
    }

    [Fact]
    public void CreateError_WhenNotAuthorizedError_ShouldHaveErrorTypeUnauthorized()
    {
        var error = Error.Unauthorized(ErrorCode, ErrorDescription);

        ValidateError(error, expectedErrorType: ErrorType.Unauthorized);
    }

    [Fact]
    public void CreateError_WhenCustomType_ShouldHaveCustomErrorType()
    {
        var error = Error.Custom(1232, ErrorCode, ErrorDescription);

        ValidateError(error, expectedErrorType: (ErrorType)1232);
    }

    private static void ValidateError(Error error, ErrorType expectedErrorType)
    {
        error.Code.Should().Be(ErrorCode);
        error.Description.Should().Be(ErrorDescription);
        error.Type.Should().Be(expectedErrorType);
        error.NumericType.Should().Be((int)expectedErrorType);
    }
}