namespace Max.BotClient.DTOs
{
    /// <summary>
    /// Тип элемента разметки.
    /// </summary>
    internal enum MarkupType
    {
        Strong,
        Emphasized,
        Monospaced,
        Link,
        Strikethrough,
        Underline,
        UserMention
    }

    /// <summary>
    /// Интерфейс элемента разметки сообщения.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal interface IMarkupElement
    {
        /// <summary>
        /// Тип элемента разметки.
        /// </summary>
        MarkupType Type { get; }

        /// <summary>
        /// Индекс начала элемента разметки в тексте. Нумерация с нуля.
        /// </summary>
        int From { get; }

        /// <summary>
        /// Длина элемента разметки.
        /// </summary>
        int Length { get; }
    }

    /// <summary>
    /// Элемент разметки - жирный текст.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class StrongMarkupElement : IMarkupElement
    {
        /// <inheritdoc />
        public MarkupType Type => MarkupType.Strong;

        /// <summary>
        /// Индекс начала элемента разметки в тексте. Нумерация с нуля.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Длина элемента разметки.
        /// </summary>
        public int Length { get; set; }
    }

    /// <summary>
    /// Элемент разметки - курсив.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class EmphasizedMarkupElement : IMarkupElement
    {
        /// <inheritdoc />
        public MarkupType Type => MarkupType.Emphasized;

        /// <summary>
        /// Индекс начала элемента разметки в тексте. Нумерация с нуля.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Длина элемента разметки.
        /// </summary>
        public int Length { get; set; }
    }

    /// <summary>
    /// Элемент разметки - моноширинный текст.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class MonospacedMarkupElement : IMarkupElement
    {
        /// <inheritdoc />
        public MarkupType Type => MarkupType.Monospaced;

        /// <summary>
        /// Индекс начала элемента разметки в тексте. Нумерация с нуля.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Длина элемента разметки.
        /// </summary>
        public int Length { get; set; }
    }

    /// <summary>
    /// Элемент разметки - ссылка.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class LinkMarkupElement : IMarkupElement
    {
        /// <inheritdoc />
        public MarkupType Type => MarkupType.Link;

        /// <summary>
        /// Индекс начала элемента разметки в тексте. Нумерация с нуля.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Длина элемента разметки.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// URL ссылки (1-2048 символов).
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// Элемент разметки - зачёркнутый текст.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class StrikethroughMarkupElement : IMarkupElement
    {
        /// <inheritdoc />
        public MarkupType Type => MarkupType.Strikethrough;

        /// <summary>
        /// Индекс начала элемента разметки в тексте. Нумерация с нуля.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Длина элемента разметки.
        /// </summary>
        public int Length { get; set; }
    }

    /// <summary>
    /// Элемент разметки - подчёркнутый текст.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class UnderlineMarkupElement : IMarkupElement
    {
        /// <inheritdoc />
        public MarkupType Type => MarkupType.Underline;

        /// <summary>
        /// Индекс начала элемента разметки в тексте. Нумерация с нуля.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Длина элемента разметки.
        /// </summary>
        public int Length { get; set; }
    }

    /// <summary>
    /// Элемент разметки - упоминание пользователя.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class UserMentionMarkupElement : IMarkupElement
    {
        /// <inheritdoc />
        public MarkupType Type => MarkupType.UserMention;

        /// <summary>
        /// Индекс начала элемента разметки в тексте. Нумерация с нуля.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Длина элемента разметки.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// @username упомянутого пользователя.
        /// </summary>
        public string? UserLink { get; set; }

        /// <summary>
        /// Идентификатор упомянутого пользователя.
        /// </summary>
        public long? UserId { get; set; }
    }
}
