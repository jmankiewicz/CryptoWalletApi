using CryptoWalletApi.Middlewares.Exceptions;

namespace CryptoWalletApi.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch(ForbiddenException forbiddenException)
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(forbiddenException.Message);
        }
        catch (ArgumentException argumentException)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(argumentException.Message);
        }
        catch (BadHttpRequestException badRequestException)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(badRequestException.Message);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(exception.Message);
        }
    }
}
