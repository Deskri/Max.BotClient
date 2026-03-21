namespace Max.BotClient.DTOs
{
    /// <summary>
    /// Ответ метода GET /messages.
    /// </summary>
    internal class GetMessagesResponse
    {
        /// <summary>
        /// Массив сообщений.
        /// </summary>
        public Message[]? Messages { get; set; }
    }

    /// <summary>
    /// Ответ метода POST /messages.
    /// </summary>
    internal class SendMessageResponse
    {
        /// <summary>
        /// Отправленное сообщение.
        /// </summary>
        public Message? Message { get; set; }
    }

    /// <summary>
    /// Сообщение в чате.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class Message
    {
        /// <summary>
        /// Пользователь, отправивший сообщение.
        /// </summary>
        public User? Sender { get; set; }

        /// <summary>
        /// Получатель сообщения. Может быть пользователем или чатом.
        /// </summary>
        public Recipient Recipient { get; set; }

        /// <summary>
        /// Время создания сообщения в формате Unix-time.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Пересланное или ответное сообщение.
        /// </summary>
        public LinkedMessage? Link { get; set; }

        /// <summary>
        /// Содержимое сообщения. Текст + вложения.
        /// </summary>
        public MessageBody? Body { get; set; }

        /// <summary>
        /// Статистика сообщения.
        /// </summary>
        public MessageStat? Stat { get; set; }

        /// <summary>
        /// Публичная ссылка на пост в канале. Отсутствует для диалогов и групповых чатов.
        /// </summary>
        public string? Url { get; set; }
    }

    /// <summary>
    /// Получатель сообщения.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class Recipient
    {
        /// <summary>
        /// ID чата.
        /// </summary>
        public long? ChatId { get; set; }

        /// <summary>
        /// Тип чата.
        /// </summary>
        public ChatType ChatType { get; set; }

        /// <summary>
        /// ID пользователя.
        /// </summary>
        public long? UserId { get; set; }
    }

    /// <summary>
    /// Тип связи сообщения.
    /// </summary>
    internal enum MessageLinkType
    {
        Forward,
        Reply
    }

    /// <summary>
    /// Пересланное или ответное сообщение.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class LinkedMessage
    {
        /// <summary>
        /// Тип связанного сообщения.
        /// </summary>
        public MessageLinkType Type { get; set; }

        /// <summary>
        /// Пользователь, отправивший сообщение.
        /// </summary>
        public User? Sender { get; set; }

        /// <summary>
        /// Чат, в котором сообщение было изначально опубликовано. Только для пересланных сообщений.
        /// </summary>
        public long? ChatId { get; set; }

        /// <summary>
        /// Тело сообщения.
        /// </summary>
        public MessageBody Message { get; set; }
    }

    /// <summary>
    /// Схема, представляющая тело сообщения.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class MessageBody
    {
        /// <summary>
        /// Уникальный ID сообщения.
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        /// ID последовательности сообщения в чате.
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Вложения сообщения. Могут быть одним из типов Attachment.
        /// </summary>
        public IAttachment[]? Attachments { get; set; }

        /// <summary>
        /// Разметка сообщения.
        /// </summary>
        public IMarkupElement[]? Markup { get; set; }
    }

    /// <summary>
    /// Статистика сообщения.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class MessageStat
    {
        /// <summary>
        /// Количество просмотров.
        /// </summary>
        public int Views { get; set; }
    }
}
