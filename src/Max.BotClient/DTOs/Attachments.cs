namespace Max.BotClient.DTOs
{
    /// <summary>
    /// Тип вложения.
    /// </summary>
    internal enum AttachmentType
    {
        Image,
        Video,
        Audio,
        File,
        Sticker,
        Contact,
        Share,
        Location,
        InlineKeyboard
    }

    /// <summary>
    /// Интерфейс вложения.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal interface IAttachment
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        AttachmentType Type { get; }
    }

    /// <summary>
    /// Вложение с изображением.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class PhotoAttachment : IAttachment
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Image;

        /// <summary>
        /// Данные изображения.
        /// </summary>
        public PhotoAttachmentPayload Payload { get; set; }
    }

    /// <summary>
    /// Данные вложения с изображением.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class PhotoAttachmentPayload
    {
        /// <summary>
        /// Уникальный ID изображения.
        /// </summary>
        public long PhotoId { get; set; }

        /// <summary>
        /// Токен для повторного использования вложения.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// URL изображения.
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// Вложение с видео.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class VideoAttachment : IAttachment
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Video;

        /// <summary>
        /// Данные видео.
        /// </summary>
        public MediaAttachmentPayload Payload { get; set; }

        /// <summary>
        /// Миниатюра видео.
        /// </summary>
        public VideoThumbnail? Thumbnail { get; set; }

        /// <summary>
        /// Ширина видео.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Высота видео.
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Длина видео в секундах.
        /// </summary>
        public int? Duration { get; set; }
    }

    /// <summary>
    /// Вложение с аудио.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class AudioAttachment : IAttachment
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Audio;

        /// <summary>
        /// Данные аудио.
        /// </summary>
        public MediaAttachmentPayload Payload { get; set; }

        /// <summary>
        /// Аудио транскрипция.
        /// </summary>
        public string? Transcription { get; set; }
    }

    /// <summary>
    /// Вложение с файлом.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class FileAttachment : IAttachment
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.File;

        /// <summary>
        /// Данные файла.
        /// </summary>
        public FileAttachmentPayload Payload { get; set; }

        /// <summary>
        /// Имя загруженного файла.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Размер файла в байтах.
        /// </summary>
        public long Size { get; set; }
    }

    /// <summary>
    /// Данные вложения с файлом.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class FileAttachmentPayload
    {
        /// <summary>
        /// URL файла.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Токен для повторного использования вложения.
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// Вложение со стикером.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class StickerAttachment : IAttachment
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Sticker;

        /// <summary>
        /// Данные стикера.
        /// </summary>
        public StickerAttachmentPayload Payload { get; set; }

        /// <summary>
        /// Ширина стикера.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота стикера.
        /// </summary>
        public int Height { get; set; }
    }

    /// <summary>
    /// Данные вложения со стикером.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class StickerAttachmentPayload
    {
        /// <summary>
        /// URL стикера.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// ID стикера.
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// Вложение с контактом.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class ContactAttachment : IAttachment
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Contact;

        /// <summary>
        /// Данные контакта.
        /// </summary>
        public ContactAttachmentPayload Payload { get; set; }
    }

    /// <summary>
    /// Данные вложения с контактом.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class ContactAttachmentPayload
    {
        /// <summary>
        /// Информация о пользователе в формате VCF.
        /// </summary>
        public string? VcfInfo { get; set; }

        /// <summary>
        /// Информация о пользователе.
        /// </summary>
        public User? MaxInfo { get; set; }
    }

    /// <summary>
    /// Вложение с предпросмотром ссылки.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class ShareAttachment : IAttachment
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.Share;

        /// <summary>
        /// Данные ссылки.
        /// </summary>
        public ShareAttachmentPayload Payload { get; set; }

        /// <summary>
        /// Заголовок предпросмотра ссылки.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Описание предпросмотра ссылки.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Изображение предпросмотра ссылки.
        /// </summary>
        public string? ImageUrl { get; set; }
    }

    /// <summary>
    /// Данные вложения с предпросмотром ссылки.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class ShareAttachmentPayload
    {
        /// <summary>
        /// URL, прикрепленный к сообщению в качестве предпросмотра медиа.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Токен вложения.
        /// </summary>
        public string? Token { get; set; }
    }

    /// <summary>
    /// Вложение с геолокацией.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class LocationAttachment : IAttachment
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
    /// Вложение с inline-клавиатурой.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class InlineKeyboardAttachment : IAttachment
    {
        /// <inheritdoc />
        public AttachmentType Type => AttachmentType.InlineKeyboard;

        /// <summary>
        /// Клавиатура.
        /// </summary>
        public Keyboard Payload { get; set; }
    }

    /// <summary>
    /// Данные медиа-вложения.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class MediaAttachmentPayload
    {
        /// <summary>
        /// URL медиа-вложения.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Токен для повторного использования вложения.
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// Миниатюра видео.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class VideoThumbnail
    {
        /// <summary>
        /// URL изображения миниатюры.
        /// </summary>
        public string Url { get; set; }
    }
}
