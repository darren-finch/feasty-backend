using System;
using System.Net;
using new_backend.Exceptions;

namespace new_backend.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlerMiddleware> logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred: " + e.Message);
                
                context.Response.StatusCode = GetStatusCode(e);

                await context.Response.WriteAsJsonAsync(
                    new ErrorResponse(
                        Message: "An error occurred when processing your request.",
                        Details: e.Message
                    )
                );
            }
        }

        private static int GetStatusCode(Exception exception)
        {
            switch (exception)
            {
                case NotFoundException _:
                    return (int)HttpStatusCode.NotFound;
                case UnauthorizedException _:
                    return (int)HttpStatusCode.Unauthorized;
                case BadRequestException _:
                    return (int)HttpStatusCode.BadRequest;
                default:
                    return (int)HttpStatusCode.InternalServerError;
            }
        }

    }
}

