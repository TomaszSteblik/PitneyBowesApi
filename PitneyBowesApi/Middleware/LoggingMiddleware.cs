using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PitneyBowesApi.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var log = $"REQUEST {DateTime.Now}:";
            log += await GetLogRequest(httpContext.Request);

            var ogBody = httpContext.Response.Body;
            httpContext.Response.Body = new MemoryStream();
            
            await _next.Invoke(httpContext);
            
            log += $"\nRESPONSE {DateTime.Now}:";
            log += await GetLogResponse(httpContext.Response);
            
            await httpContext.Response.Body.CopyToAsync(ogBody);
            
            _logger.LogInformation(log);
        }
        private async Task<string> GetLogRequest(HttpRequest request)
        {
            
            request.EnableBuffering();
            var streamReader = new StreamReader(request.Body);
            var body = await streamReader.ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
            body = body.Replace("{", "{\n\t");
            body = body.Replace(",", ",\n\t");
            body = body.Replace("}", "\n}");

            var headers = "";
            foreach (var header in request.Headers)
            {
                headers += $"{header.Key}: {header.Value}\n";
            }
            
            return $"\nHeaders:\n{headers}Body:\n{body}\n";
            
        }
        private async Task<string> GetLogResponse(HttpResponse response)
        {
            
            response.Body.Seek(0, SeekOrigin.Begin);
            var streamReader = new StreamReader(response.Body);
            var body = await streamReader.ReadToEndAsync();            
            response.Body.Seek(0, SeekOrigin.Begin);
            body = body.Replace("{", "{\n\t");
            body = body.Replace(",", ",\n\t");
            body = body.Replace("}", "\n}");

            var headers = "";
            foreach (var header in response.Headers)
            {
                headers += $"{header.Key}: {header.Value}\n";
            }
            
            return $"\nHeaders:\nStatus code: {response.StatusCode}\n{headers}Body:\n{body}\n";
        }

       
    }
}