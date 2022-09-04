using System.Diagnostics;

namespace Assessment.WebApi.Middlewares
{
    public class RequestExcecutionTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestExcecutionTimeMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger(typeof(RequestExcecutionTimeMiddleware));
        }

        public async Task Invoke(HttpContext context)
        {
            Guid traceID = Guid.NewGuid();

            _logger.LogInformation($"Solicitud {traceID} iniciada");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            await _next(context);

            stopWatch.Stop();

            TimeSpan timeSpan = stopWatch.Elapsed;

            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);

            _logger.LogDebug($"La solicitud {traceID} ha tardado {elapsedTime}");
        }
    }
}