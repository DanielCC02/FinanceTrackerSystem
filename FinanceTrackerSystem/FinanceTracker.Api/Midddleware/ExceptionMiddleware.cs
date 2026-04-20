using System.Net;
using System.Text.Json;
using FluentValidation;

namespace FinanceTracker.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;

                _logger.LogError(ex, "Error TraceId: {TraceId}", traceId);

                context.Response.ContentType = "application/json";

                object response;

                switch (ex)
                {
                    case ValidationException validationEx:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                        response = new
                        {
                            message = "Validation failed",
                            traceId,
                            errors = validationEx.Errors.Select(e => new
                            {
                                field = e.PropertyName,
                                error = e.ErrorMessage
                            })
                        };
                        break;

                    case UnauthorizedAccessException:
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                        response = new
                        {
                            message = ex.Message,
                            traceId
                        };
                        break;

                    case KeyNotFoundException:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                        response = new
                        {
                            message = ex.Message,
                            traceId
                        };
                        break;

                    case ArgumentException:
                    case InvalidOperationException:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                        response = new
                        {
                            message = ex.Message,
                            traceId
                        };
                        break;

                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        response = new
                        {
                            message = "Internal server error",
                            traceId
                        };
                        break;
                }

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}