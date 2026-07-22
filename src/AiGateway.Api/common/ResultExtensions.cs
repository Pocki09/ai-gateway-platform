using AiGateway.SharedKenel.Results;
using Microsoft.AspNetCore.Mvc;

namespace AiGateway.Api.Common;

/// <summary>
/// Extension methods để convert Result → IActionResult.
/// Tập trung logic mapping error code → HTTP status code tại đây.
/// Controller chỉ cần gọi result.ToActionResult() — sạch và nhất quán.
/// </summary>
public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(new ApiResponse<T>(true, result.value, null));

        return MapErrorToResponse(result.Error);
    }

    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(new ApiResponse<object>(true, null, null));

        return MapErrorToResponse(result.Error);
    }

    public static IActionResult ToCreatedResult<T>(this Result<T> result, string routeName, object routeValues)
    {
        if (result.IsSuccess)
        {
            var response = new ApiResponse<T>(true, result.value, null);
            return new CreatedAtRouteResult(routeName, routeValues, response);
        }

        return MapErrorToResponse(result.Error);
    }

    private static IActionResult MapErrorToResponse(Error error)
    {
        // Map error code prefix → HTTP status code
        var statusCode = error.Code switch
        {
            var c when c.EndsWith(".NotFound") => 404,
            var c when c.EndsWith(".Unauthorized") => 401,
            var c when c.EndsWith(".Forbidden") => 403,
            var c when c.EndsWith(".AlreadyExists") || c.EndsWith(".SlugAlreadyExists") => 409,
            _ => 400
        };

        var apiError = new ApiError(error.Code, error.Message);
        return new ObjectResult(new ApiResponse<object>(false, null, apiError))
        {
            StatusCode = statusCode
        };
    }
}