namespace BaCS.Presentation.API.Middlewares;

using Application.Contracts.Exceptions;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

public class ApplicationExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<ApplicationExceptionHandler> logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        LogException(httpContext, exception);

        return await HandleException(httpContext, exception);
    }

    private void LogException(HttpContext httpContext, Exception exception)
    {
        switch (exception)
        {
            case AggregateException aggregateException
                when aggregateException.InnerExceptions.Any(x => x is ApplicationException):
            case ApplicationException or OperationCanceledException or ConnectionResetException:
                {
                    logger.LogWarning(
                        exception,
                        "HTTP {RequestMethod} {RequestPath} completed with {ErrorType}: {UnhandledError}",
                        httpContext.Request.Method,
                        httpContext.Request.Path,
                        exception.GetType().Name,
                        exception.Message
                    );

                    break;
                }
            default:
                {
                    logger.LogError(
                        exception,
                        "HTTP {RequestMethod} {RequestPath} completed with {ErrorType}: {UnhandledError}",
                        httpContext.Request.Method,
                        httpContext.Request.Path,
                        exception.GetType().Name,
                        exception.Message
                    );

                    break;
                }
        }
    }

    private async Task<bool> HandleException(HttpContext httpContext, Exception exception)
    {
        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ForbiddenException => StatusCodes.Status403Forbidden,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            FluentValidation.ValidationException
                or ApplicationException
                or ArgumentException
                or AggregateException
                or NotImplementedException
                => StatusCodes.Status400BadRequest,
            OperationCanceledException or ConnectionResetException => StatusCodes.Status499ClientClosedRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var title = exception switch
        {
            NotImplementedException => "Функционал находится в разработке",
            FluentValidation.ValidationException or ValidationException => "Ошибка валидации параметров запроса",
            NotFoundException => "Объект не найден",
            ForbiddenException => "Недостаточно прав",
            UnauthorizedException => "Ошибка аутентификации",
            ApplicationException => "Ошибка выполнения запроса",
            OperationCanceledException or ConnectionResetException => "Запрос отменён клиентом",
            _ => "Внутренняя ошибка"
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = exception.GetType().Name,
            Detail = statusCode switch
            {
                >= StatusCodes.Status500InternalServerError => "Не смогли обработать запрос из-за ошибки",
                _ => exception.Message
            }
        };

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                Exception = exception,
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            }
        );
    }
}
