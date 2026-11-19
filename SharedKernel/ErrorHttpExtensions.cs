namespace RoomHub.Api.SharedKernel;

public static class ErrorHttpExtensions
{
    /// <summary>The single place where an Error becomes an HTTP ProblemDetails response.</summary>
    public static IResult ToProblem(this Error error) =>
        Results.Problem(
            statusCode: error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            },
            title: error.Code,
            detail: error.Description);
}
