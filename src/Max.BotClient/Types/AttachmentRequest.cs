namespace Max.BotClient.Types
{
    /// <summary>
    /// Тип вложения для отправки.
    /// </summary>
    public enum AttachmentRequestType
    {
        Image,
        Video,
        Audio,
        File,
        Sticker,
        Contact,
        Location,
        Share,
        InlineKeyboard
    }

    /// <summary>
    /// Вложение для отправки в сообщении.
    /// <see href="https://dev.max.ru/docs-api/objects/AttachmentRequest"/>
    /// </summary>
    public class AttachmentRequest
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        public AttachmentRequestType Type { get; set; }

        /// <summary>
        /// Данные вложения.
        /// </summary>
        public AttachmentPayload? Payload { get; set; }
    }

    /// <summary>
    /// Данные вложения.
    /// </summary>
    public class AttachmentPayload
    {
        // === Image/Video/Audio/File ===

        /// <summary>
        /// URL файла для загрузки (для image).
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Токен загруженного файла.
        /// </summary>
        public string? Token { get; set; }

        // === Sticker ===

        /// <summary>
        /// Код стикера.
        /// </summary>
        public string? Code { get; set; }

        // === Contact ===

        /// <summary>
        /// Имя контакта.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// ID контакта (если пользователь MAX).
        /// </summary>
        public long? ContactId { get; set; }

        /// <summary>
        /// Информация о контакте в формате VCF.
        /// </summary>
        public string? VcfInfo { get; set; }

        /// <summary>
        /// Телефон контакта.
        /// </summary>
        public string? VcfPhone { get; set; }

        // === Location ===

        /// <summary>
        /// Широта.
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Долгота.
        /// </summary>
        public double? Longitude { get; set; }

        // === Share ===

        /// <summary>
        /// URL для share.
        /// </summary>
        public string? ShareUrl { get; set; }

        // === InlineKeyboard ===

        /// <summary>
        /// Кнопки клавиатуры.
        /// </summary>
        public ButtonRequest[][]? Buttons { get; set; }
    }

    /// <summary>
    /// Тип кнопки для отправки.
    /// </summary>
    public enum ButtonRequestType
    {
        Callback,
        Link,
        RequestGeoLocation,
        RequestContact,
        OpenApp,
        Message
    }

    /// <summary>
    /// Кнопка для отправки.
    /// </summary>
    public class ButtonRequest
    {
        /// <summary>
        /// Тип кнопки.
        /// </summary>
        public ButtonRequestType Type { get; set; }

        /// <summary>
        /// Видимый текст кнопки.
        /// </summary>
        public string Text { get; set; } = null!;

        /// <summary>
        /// Payload данные для callback.
        /// </summary>
        public string? Payload { get; set; }

        /// <summary>
        /// URL для кнопки-ссылки.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Быстрая отправка геолокации без подтверждения.
        /// </summary>
        public bool? Quick { get; set; }

        /// <summary>
        /// Username бота для мини-приложения.
        /// </summary>
        public string? WebApp { get; set; }

        /// <summary>
        /// ID чата для мини-приложения.
        /// </summary>
        public long? ChatId { get; set; }
    }
}
