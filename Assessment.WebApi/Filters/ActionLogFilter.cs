using Microsoft.AspNetCore.Mvc.Filters;

namespace Assessment.WebApi.Filters
{
    public class ActionLogFilter : IActionFilter
    {
        private readonly ILogger<ActionLogFilter> _logger;

        public ActionLogFilter(ILogger<ActionLogFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Login de usuario");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("El usuario se ha logeado con éxito.");
        }
    }
}
