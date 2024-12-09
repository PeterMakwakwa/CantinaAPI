using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CantinaAPI.Middlewares
{
    /// <summary>
    /// Middleware for handling exceptions and returning a standardized error response.
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="next">The next middleware in the pipeline.</param>
        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware to handle exceptions.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handles the exception and writes a standardized error response.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorId = Guid.NewGuid(); // Log error identifier
            _logger.LogError(exception, "Error ID: {ErrorId} - {Message}", errorId, exception.Message);

            var response = context.Response;
            response.ContentType = "application/json";

            var (statusCode, errorMessage) = GetErrorResponse(exception);

            response.StatusCode = statusCode;

            var errorResponse = new
            {
                Id = errorId,
                ErrorMessage = errorMessage,
            };

            // Write response
            await response.WriteAsJsonAsync(errorResponse);
        }

        /// <summary>
        /// Determines the status code and error message based on the exception type.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>A tuple containing the status code and error message.</returns>
        private (int statusCode, string errorMessage) GetErrorResponse(Exception exception)
        {
            return exception switch
            {
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "The requested resource was not found."),
                ArgumentException => ((int)HttpStatusCode.BadRequest, "The request was invalid. Please check the inputs."),
                _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred. Please contact support."),
            };
        }
    }
}
