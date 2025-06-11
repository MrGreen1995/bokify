using Serilog.Context;


namespace Bookify.Api.Middleware
{
    public class RequestContextLoggingMiddleware(RequestDelegate next)
    {
        private const string CorrelationIdHeader = "X-Correlation-Id";        

        public Task InvokeAsync(HttpContext httpContext)
        {
            using (LogContext.PushProperty("CorrelationId", GetCorrelationId(httpContext)))
            {
                return next.Invoke(httpContext);
            }
        }

        private static string GetCorrelationId(HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId);
            
            return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
        }
    }
}
