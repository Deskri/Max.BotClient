namespace Max.BotClient.Types
{
    /// <summary>
    /// Формат текста сообщения.
    /// </summary>
    public enum TextFormat
    {
        Markdown,
        Html
    }

    /// <summary>
    /// Тип ссылки на сообщение.
    /// </summary>
    public enum MessageLinkRequestType
    {
        Reply,
        Forward
    }

    /// <summary>
    /// Ссылка на сообщение для отправки.
    /// </summary>
    public class NewMessageLink
    {
        /// <summary>
        /// Тип ссылки.
        /// </summary>
        public MessageLinkRequestType Type { get; set; }

        /// <summary>
        /// ID сообщения, на которое ссылается.
        /// </summary>
        public string Mid { get; set; } = null!;
    }

    /// <summary>
    /// Ответ на отправку сообщения.
    /// </summary>
    public class SendMessageResponse
    {
        /// <summary>
        /// Отправленное сообщение.
        /// </summary>
        public Message? Message { get; set; }
    }
}
