namespace Max.BotClient.Types
{
    /// <summary>
    /// Тело нового сообщения для отправки.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    public class NewMessageBody
    {
        /// <summary>
        /// Текст сообщения (до 4000 символов).
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Вложения сообщения.
        /// </summary>
        public AttachmentRequest[]? Attachments { get; set; }

        /// <summary>
        /// Ссылка на сообщение (reply/forward).
        /// </summary>
        public NewMessageLink? Link { get; set; }

        /// <summary>
        /// Уведомлять участников чата (по умолчанию true).
        /// </summary>
        public bool? Notify { get; set; }

        /// <summary>
        /// Формат текста (markdown/html).
        /// </summary>
        public TextFormat? Format { get; set; }
    }
}
