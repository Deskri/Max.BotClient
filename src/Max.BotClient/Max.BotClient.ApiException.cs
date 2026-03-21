using System;
using System.Net;

namespace Max.BotClient
{
    /// <summary>
    /// Исключение, возникающее при ошибке API.
    /// </summary>
    public class MaxBotClientApiException : Exception
    {
        /// <summary>
        /// HTTP статус код ответа.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Тело ответа с ошибкой.
        /// </summary>
        public string? ResponseBody { get; }

        /// <summary>
        /// Можно ли повторить запрос.
        /// </summary>
        public bool IsRetryable => (int)StatusCode == 429 || (int)StatusCode >= 500;

        public MaxBotClientApiException(
            HttpStatusCode statusCode, 
            string? responseBody = null
        ) : base($"API request failed with status {(int)statusCode} ({statusCode})")
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }

        public MaxBotClientApiException(
            HttpStatusCode statusCode, 
            string? responseBody, 
            Exception innerException
        ) : base($"API request failed with status {(int)statusCode} ({statusCode})", innerException)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }
    }
}
