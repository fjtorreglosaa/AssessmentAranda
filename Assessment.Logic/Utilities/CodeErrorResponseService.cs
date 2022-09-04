namespace Assessment.Logic.Utilities
{
    public class CodeErrorResponseService
    {
        public CodeErrorResponseService(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageStatusCode(statusCode);
        }

        private string GetDefaultMessageStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "El request enviado tiene errores",
                401 => "El usuario no esta autorizado para acceder al recurso",
                404 => "El recurso solicitado no se encontró",
                500 => "Se han producido errores en el servidor", _ => null
            };
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
