namespace Max.BotClient.DTOs
{
    /// <summary>
    /// Базовый ответ API.
    /// </summary>
    internal class ApiResponse
    {
        /// <summary>
        /// true, если запрос был успешным, false в противном случае.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Объяснительное сообщение, если результат не был успешным.
        /// </summary>
        public string? Message { get; set; }
    }

    /// <summary>
    /// Формат текста сообщения.
    /// </summary>
    internal enum TextFormat
    {
        Markdown,
        Html
    }

    /// <summary>
    /// Тело нового сообщения.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class NewMessageBody
    {
        /// <summary>
        /// Текст сообщения (до 4000 символов).
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Вложения сообщения. Если пусто, все вложения будут удалены.
        /// </summary>
        public IAttachmentRequest[]? Attachments { get; set; }

        /// <summary>
        /// Ссылка на сообщение.
        /// </summary>
        public NewMessageLink? Link { get; set; }

        /// <summary>
        /// Если false, участники чата не будут уведомлены (по умолчанию true).
        /// </summary>
        public bool? Notify { get; set; }

        /// <summary>
        /// Формат текста сообщения.
        /// </summary>
        public TextFormat? Format { get; set; }
    }

    /// <summary>
    /// Ссылка на сообщение.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class NewMessageLink
    {
        /// <summary>
        /// Тип ссылки сообщения.
        /// </summary>
        public MessageLinkType Type { get; set; }

        /// <summary>
        /// ID исходного сообщения.
        /// </summary>
        public string Mid { get; set; }
    }

    /// <summary>
    /// Интерфейс запроса вложения.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal interface IAttachmentRequest
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        AttachmentType Type { get; }
    }

    /// <summary>
    /// Запрос на прикрепление изображения.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class PhotoAttachmentRequest : IAttachmentRequest
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Image;

        /// <summary>
        /// Данные изображения.
        /// </summary>
        public PhotoAttachmentRequestPayload Payload { get; set; }
    }

    /// <summary>
    /// Данные запроса на прикрепление изображения (все поля являются взаимоисключающими).
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class PhotoAttachmentRequestPayload
    {
        /// <summary>
        /// Любой внешний URL изображения, которое вы хотите прикрепить.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Токен существующего вложения.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Токены, полученные после загрузки изображений.
        /// </summary>
        public PhotoToken? Photos { get; set; }
    }

    /// <summary>
    /// Токен загруженного изображения.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class PhotoToken
    {
        /// <summary>
        /// Закодированная информация загруженного изображения.
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// Запрос на прикрепление видео.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class VideoAttachmentRequest : IAttachmentRequest
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Video;

        /// <summary>
        /// Данные загруженного видео.
        /// </summary>
        public UploadedInfo Payload { get; set; }
    }

    /// <summary>
    /// Информация о загруженном аудио/видео.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class UploadedInfo
    {
        /// <summary>
        /// Токен — уникальный ID загруженного медиафайла.
        /// </summary>
        public string? Token { get; set; }
    }

    /// <summary>
    /// Запрос на прикрепление аудио.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class AudioAttachmentRequest : IAttachmentRequest
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Audio;

        /// <summary>
        /// Данные загруженного аудио.
        /// </summary>
        public UploadedInfo Payload { get; set; }
    }

    /// <summary>
    /// Запрос на прикрепление файла.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class FileAttachmentRequest : IAttachmentRequest
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.File;

        /// <summary>
        /// Данные загруженного файла.
        /// </summary>
        public UploadedInfo Payload { get; set; }
    }

    /// <summary>
    /// Запрос на прикрепление стикера.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class StickerAttachmentRequest : IAttachmentRequest
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Sticker;

        /// <summary>
        /// Данные стикера.
        /// </summary>
        public StickerAttachmentRequestPayload Payload { get; set; }
    }

    /// <summary>
    /// Данные запроса на прикрепление стикера.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class StickerAttachmentRequestPayload
    {
        /// <summary>
        /// Код стикера.
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// Запрос на прикрепление контакта.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class ContactAttachmentRequest : IAttachmentRequest
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Contact;

        /// <summary>
        /// Данные контакта.
        /// </summary>
        public ContactAttachmentRequestPayload Payload { get; set; }
    }

    /// <summary>
    /// Данные запроса на прикрепление контакта.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class ContactAttachmentRequestPayload
    {
        /// <summary>
        /// Имя контакта.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// ID контакта, если он зарегистрирован в MAX.
        /// </summary>
        public long? ContactId { get; set; }

        /// <summary>
        /// Полная информация о контакте в формате VCF.
        /// </summary>
        public string? VcfInfo { get; set; }

        /// <summary>
        /// Телефон контакта в формате VCF.
        /// </summary>
        public string? VcfPhone { get; set; }
    }

    /// <summary>
    /// Запрос на прикрепление inline-клавиатуры.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class InlineKeyboardAttachmentRequest : IAttachmentRequest
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.InlineKeyboard;

        /// <summary>
        /// Данные клавиатуры.
        /// </summary>
        public InlineKeyboardAttachmentRequestPayload Payload { get; set; }
    }

    /// <summary>
    /// Данные запроса на прикрепление inline-клавиатуры.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class InlineKeyboardAttachmentRequestPayload
    {
        /// <summary>
        /// Двумерный массив кнопок.
        /// </summary>
        public IButton[][] Buttons { get; set; }
    }

    /// <summary>
    /// Запрос на прикрепление геолокации.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class LocationAttachmentRequest : IAttachmentRequest
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Location;

        /// <summary>
        /// Широта.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота.
        /// </summary>
        public double Longitude { get; set; }
    }

    /// <summary>
    /// Запрос на прикрепление ссылки.
    /// <see href="https://dev.max.ru/docs-api/objects/NewMessageBody"/>
    /// </summary>
    internal class ShareAttachmentRequest : IAttachmentRequest
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Share;

        /// <summary>
        /// Данные ссылки.
        /// </summary>
        public ShareAttachmentPayload Payload { get; set; }
    }
}
