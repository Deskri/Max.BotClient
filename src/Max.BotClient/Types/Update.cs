namespace Max.BotClient.Types
{
    /// <summary>
    /// Обновление от API.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    public class Update
    {
        /// <summary>
        /// Тип обновления.
        /// </summary>
        public UpdateType UpdateType { get; set; }

        /// <summary>
        /// Unix-время события.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Сообщение (для MessageCreated, MessageCallback, MessageEdited).
        /// </summary>
        public Message? Message { get; set; }

        /// <summary>
        /// Данные callback (для MessageCallback).
        /// </summary>
        public Callback? Callback { get; set; }

        /// <summary>
        /// ID сообщения (для MessageRemoved).
        /// </summary>
        public string? MessageId { get; set; }

        /// <summary>
        /// ID чата.
        /// </summary>
        public long? ChatId { get; set; }

        /// <summary>
        /// ID пользователя (для MessageRemoved).
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Является ли чат каналом (для BotAdded, BotRemoved, UserAdded, UserRemoved).
        /// </summary>
        public bool? IsChannel { get; set; }

        /// <summary>
        /// Время окончания mute (для DialogMuted).
        /// </summary>
        public long? MutedUntil { get; set; }

        /// <summary>
        /// ID пригласившего пользователя (для UserAdded).
        /// </summary>
        public long? InviterId { get; set; }

        /// <summary>
        /// ID администратора (для UserRemoved).
        /// </summary>
        public long? AdminId { get; set; }

        /// <summary>
        /// Payload данные (для BotStarted).
        /// </summary>
        public string? Payload { get; set; }

        /// <summary>
        /// Новое название чата (для ChatTitleChanged).
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Локаль пользователя в формате IETF BCP 47.
        /// </summary>
        public string? UserLocale { get; set; }
    }

    /// <summary>
    /// Ответ с обновлениями.
    /// </summary>
    internal class GetUpdatesResponse
    {
        /// <summary>
        /// Список обновлений.
        /// </summary>
        public Update[] Updates { get; set; } = [];

        /// <summary>
        /// Маркер для следующего запроса.
        /// </summary>
        public long? Marker { get; set; }
    }
}
