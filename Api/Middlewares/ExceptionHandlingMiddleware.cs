using Application.Exceptions;

namespace Api.Middlewares;

public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обработке запроса");

            context.Response.StatusCode = ex switch
            {
                AppException appEx => appEx.StatusCode,
                _ => 500
            };

            await context.Response.WriteAsync(ex.Message);
        }
    }
}