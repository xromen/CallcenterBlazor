namespace Callcenter.Api.Middlewares;

public class LogRequestBodyMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ILogger<LogRequestBodyMiddleware> logger)
    {
        context.Request.EnableBuffering();
        var requestBody = context.Request.Body;
        var body = await new StreamReader(requestBody).ReadToEndAsync();
        requestBody.Position = 0;
        logger.LogDebug(body);
        await next(context);
    }
}