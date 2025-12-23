using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Core.Middleware
{
    public class SerilogLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SerilogLoggingMiddleware> _logger;

        public SerilogLoggingMiddleware(RequestDelegate next, ILogger<SerilogLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString();

            _logger.LogInformation(
                "Request: {RequestId} - {Method} {Path}",
                requestId, context.Request.Method, context.Request.Path);

            await _next(context);

            stopwatch.Stop();
            _logger.LogInformation(
                "Response: {RequestId} - {StatusCode} - {ElapsedMs}ms",
                requestId, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }


        //More advanced and detailed version with request and response body logging
        //public async Task InvokeAsync2(HttpContext context)
        //{
        //    // Generate a unique request ID
        //    var requestId = Guid.NewGuid().ToString();
        //    using (LogContext.PushProperty("RequestId", requestId))
        //    {
        //        var stopwatch = Stopwatch.StartNew();

        //        // Log incoming request
        //        await LogRequest(context, requestId);

        //        // Copy the response stream to read it later
        //        var originalBodyStream = context.Response.Body;
        //        using (var responseBody = new MemoryStream())
        //        {
        //            context.Response.Body = responseBody;

        //            try
        //            {
        //                // Call the next middleware
        //                await _next(context);

        //                stopwatch.Stop();

        //                // Log response
        //                await LogResponse(context, requestId, stopwatch.ElapsedMilliseconds);

        //                // Copy the response to the original stream
        //                await responseBody.CopyToAsync(originalBodyStream);
        //            }
        //            catch (Exception ex)
        //            {
        //                stopwatch.Stop();
        //                _logger.LogError(ex, "RequestId: {RequestId} - An unhandled exception occurred", requestId);
        //                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        //                throw;
        //            }
        //            finally
        //            {
        //                context.Response.Body = originalBodyStream;
        //            }
        //        }
        //    }
        //}

        private async Task LogRequest(HttpContext context, string requestId)
        {
            var request = context.Request;

            // Read the request body
            request.EnableBuffering();
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Position = 0;

            var logData = new
            {
                RequestId = requestId,
                Method = request.Method,
                Path = request.Path,
                QueryString = request.QueryString.ToString(),
                Headers = request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()),
                Body = body,
                RemoteIP = context.Connection.RemoteIpAddress?.ToString(),
                Timestamp = DateTime.UtcNow
            };

            _logger.LogInformation("Incoming Request: {@RequestData}", logData);
        }

        private async Task LogResponse(HttpContext context, string requestId, long elapsedMilliseconds)
        {
            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            var logData = new
            {
                RequestId = requestId,
                StatusCode = response.StatusCode,
                Headers = response.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()),
                Body = body,
                ElapsedMilliseconds = elapsedMilliseconds,
                Timestamp = DateTime.UtcNow
            };

            var logLevel = response.StatusCode >= 500 ? LogLevel.Error
                         : response.StatusCode >= 400 ? LogLevel.Warning
                         : LogLevel.Information;

            _logger.Log(logLevel, "Outgoing Response: {@ResponseData}", logData);
        }
    }
}
