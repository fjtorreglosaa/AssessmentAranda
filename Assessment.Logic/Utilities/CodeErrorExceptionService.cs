namespace Assessment.Logic.Utilities
{
    public class CodeErrorExceptionService : CodeErrorResponseService
    {
        public CodeErrorExceptionService(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }

        public string Details { get; set; }
    }
}
