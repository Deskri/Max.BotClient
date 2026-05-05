using System;

namespace Max.BotClient
{
    public class BotClientOptions
    {
        private const string BaseUrl = "https://platform-api.max.ru";
        public string Token { get; }
        public string ApiUrl { get; }
        /// <summary>
        /// Начальная задержка между повторными запросами в секундах (exponential backoff).
        /// </summary>
        public int RetryDelaySeconds { get; set; } = 1;

        /// <summary>
        /// Максимальное количество повторных попыток при ошибках 429/5xx.
        /// </summary>
        public int RetryCount { get; set; } = 3;

        /// <summary>
        /// Максимальная частота обычных API-запросов в секунду. 0 — без ограничения.
        /// </summary>
        public int RPS { get; set; } = 30;

        /// <summary>
        /// Максимальная частота polling-запросов в секунду. 0 — без ограничения.
        /// </summary>
        public int PollingRPS { get; set; } = 2;
    
        public BotClientOptions(
            string token,
            string apiUrl = null
        )
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));
            
            Token = token;
            ApiUrl = string.IsNullOrWhiteSpace(apiUrl) ? BaseUrl : apiUrl;
        }
    }
}