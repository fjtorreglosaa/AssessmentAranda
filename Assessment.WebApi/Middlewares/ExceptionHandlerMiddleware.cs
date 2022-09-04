using Assessment.Logic.Utilities;
using System.Net;
using System.Text.Json;

namespace Assessment.WebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IHostEnvironment env)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger(typeof(ExceptionHandlerMiddleware));
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                Guid traceID = Guid.NewGuid();
                _logger.LogError(e, e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new CodeErrorExceptionService((int)HttpStatusCode.InternalServerError, e.Message, traceID.ToString())
                    : new CodeErrorExceptionService((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var jsonResponse = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsJsonAsync(jsonResponse);
            }
        }
    }
}
