namespace Max.BotClient.Types
{
    /// <summary>
    /// Тип обновления.
    /// </summary>
    public enum UpdateType
    {
        MessageCreated,
        MessageCallback,
        MessageEdited,
        MessageRemoved,
        BotAdded,
        BotRemoved,
        DialogMuted,
        DialogUnmuted,
        DialogCleared,
        DialogRemoved,
        UserAdded,
        UserRemoved,
        BotStarted,
        BotStopped,
        ChatTitleChanged
    }

    /// <summary>
    /// Подписка на вебхук.
    /// <see href="https://dev.max.ru/docs-api/methods/GET/subscriptions"/>
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// URL вебхука.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Unix-время создания подписки.
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Типы обновлений, на которые подписан бот.
        /// </summary>
        public UpdateType[]? UpdateTypes { get; set; }
    }

    /// <summary>
    /// Результат операции подписки.
    /// </summary>
    public class SubscribeResult
    {
        /// <summary>
        /// true, если запрос был успешным.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Объяснительное сообщение, если результат не был успешным.
        /// </summary>
        public string? Message { get; set; }
    }

    /// <summary>
    /// Ответ со списком подписок.
    /// </summary>
    internal class GetSubscriptionsResponse
    {
        /// <summary>
        /// Список текущих подписок.
        /// </summary>
        public Subscription[] Subscriptions { get; set; } = [];
    }

    /// <summary>
    /// Запрос на подписку.
    /// </summary>
    internal class SubscribeRequest
    {
        /// <summary>
        /// URL вебхука.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Типы обновлений для подписки.
        /// </summary>
        public UpdateType[]? UpdateTypes { get; set; }

        /// <summary>
        /// Секрет для заголовка X-Max-Bot-Api-Secret.
        /// </summary>
        public string? Secret { get; set; }
    }
}
