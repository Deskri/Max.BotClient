namespace Max.BotClient.Types
{
    /// <summary>
    /// Базовый интерфейс для всех типов вложений.
    /// </summary>
    public interface IAttachment
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        AttachmentType Type { get; }
    }

    /// <summary>
    /// Вложение с изображением.
    /// </summary>
    public class PhotoAttachment : IAttachment
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        public AttachmentType Type => AttachmentType.Image;

        /// <summary>
        /// ID изображения.
        /// </summary>
        public long? PhotoId { get; internal set; }

        /// <summary>
        /// URL изображения.
        /// </summary>
        public string? Url { get; internal set; }

        /// <summary>
        /// Токен изображения.
        /// </summary>
        public string? Token { get; internal set; }

        /// <summary>
        /// Создает вложение изображения по URL.
        /// </summary>
        public static PhotoAttachment FromUrl(string url) => new PhotoAttachment { Url = url };

        /// <summary>
        /// Создает вложение изображения по токену.
        /// </summary>
        public static PhotoAttachment FromToken(string token) => new PhotoAttachment { Token = token };
    }

    /// <summary>
    /// Вложение с видео.
    /// </summary>
    public class VideoAttachment : IAttachment
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        public AttachmentType Type => AttachmentType.Video;

        /// <summary>
        /// URL видео.
        /// </summary>
        public string? Url { get; internal set; }

        /// <summary>
        /// Токен видео.
        /// </summary>
        public string? Token { get; internal set; }

        /// <summary>
        /// Ширина видео.
        /// </summary>
        public int? Width { get; internal set; }

        /// <summary>
        /// Высота видео.
        /// </summary>
        public int? Height { get; internal set; }

        /// <summary>
        /// Длительность видео в секундах.
        /// </summary>
        public int? Duration { get; internal set; }

        /// <summary>
        /// URL миниатюры видео.
        /// </summary>
        public string? ThumbnailUrl { get; internal set; }

        /// <summary>
        /// Создает вложение видео по токену.
        /// </summary>
        public static VideoAttachment FromToken(string token) => new VideoAttachment { Token = token };
    }

    /// <summary>
    /// Вложение с аудио.
    /// </summary>
    public class AudioAttachment : IAttachment
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        public AttachmentType Type => AttachmentType.Audio;

        /// <summary>
        /// URL аудио.
        /// </summary>
        public string? Url { get; internal set; }

        /// <summary>
        /// Токен аудио.
        /// </summary>
        public string? Token { get; internal set; }

        /// <summary>
        /// Транскрипция аудио.
        /// </summary>
        public string? Transcription { get; internal set; }

        /// <summary>
        /// Создает вложение аудио по токену.
        /// </summary>
        public static AudioAttachment FromToken(string token) => new AudioAttachment { Token = token };
    }

    /// <summary>
    /// Вложение с файлом.
    /// </summary>
    public class FileAttachment : IAttachment
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        public AttachmentType Type => AttachmentType.File;

        /// <summary>
        /// URL файла.
        /// </summary>
        public string? Url { get; internal set; }

        /// <summary>
        /// Токен файла.
        /// </summary>
        public string? Token { get; internal set; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string? Filename { get; internal set; }

        /// <summary>
        /// Размер файла в байтах.
        /// </summary>
        public long? Size { get; internal set; }

        /// <summary>
        /// Создает вложение файла по токену.
        /// </summary>
        public static FileAttachment FromToken(string token) => new FileAttachment { Token = token };
    }

    /// <summary>
    /// Вложение со стикером.
    /// </summary>
    public class StickerAttachment : IAttachment
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        public AttachmentType Type => AttachmentType.Sticker;

        /// <summary>
        /// URL стикера.
        /// </summary>
        public string? Url { get; internal set; }

        /// <summary>
        /// Код стикера.
        /// </summary>
        public string? Code { get; internal set; }

        /// <summary>
        /// Ширина стикера.
        /// </summary>
        public int? Width { get; internal set; }

        /// <summary>
        /// Высота стикера.
        /// </summary>
        public int? Height { get; internal set; }

        /// <summary>
        /// Создает вложение стикера по коду.
        /// </summary>
        public static StickerAttachment FromCode(string code) => new StickerAttachment { Code = code };
    }

    /// <summary>
    /// Вложение с контактом.
    /// </summary>
    public class ContactAttachment : IAttachment
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        public AttachmentType Type => AttachmentType.Contact;

        /// <summary>
        /// Информация о контакте в формате VCF.
        /// </summary>
        public string? VcfInfo { get; internal set; }

        /// <summary>
        /// Информация о пользователе контакта.
        /// </summary>
        public User? ContactUser { get; internal set; }

        /// <summary>
        /// Имя контакта.
        /// </summary>
        public string? Name { get; internal set; }

        /// <summary>
        /// ID контакта.
        /// </summary>
        public long? ContactId { get; internal set; }

        /// <summary>
        /// Телефон контакта в формате VCF.
        /// </summary>
        public string? VcfPhone { get; internal set; }

        /// <summary>
        /// Создает вложение контакта.
        /// </summary>
        public static ContactAttachment Create(string name, long? contactId = null, string? vcfInfo = null, string? vcfPhone = null) =>
            new ContactAttachment { Name = name, ContactId = contactId, VcfInfo = vcfInfo, VcfPhone = vcfPhone };
    }

    /// <summary>
    /// Вложение с предпросмотром ссылки.
    /// </summary>
    public class ShareAttachment : IAttachment
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        public AttachmentType Type => AttachmentType.Share;

        /// <summary>
        /// URL ссылки.
        /// </summary>
        public string? Url { get; internal set; }

        /// <summary>
        /// Токен.
        /// </summary>
        public string? Token { get; internal set; }

        /// <summary>
        /// Заголовок предпросмотра.
        /// </summary>
        public string? Title { get; internal set; }

        /// <summary>
        /// Описание предпросмотра.
        /// </summary>
        public string? Description { get; internal set; }

        /// <summary>
        /// URL изображения предпросмотра.
        /// </summary>
        public string? ImageUrl { get; internal set; }

        /// <summary>
        /// Создает вложение ссылки.
        /// </summary>
        public static ShareAttachment FromUrl(string url) => new ShareAttachment { Url = url };
    }

    /// <summary>
    /// Вложение с геолокацией.
    /// </summary>
    public class LocationAttachment : IAttachment
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        public AttachmentType Type => AttachmentType.Location;

        /// <summary>
        /// Широта.
        /// </summary>
        public double? Latitude { get; internal set; }

        /// <summary>
        /// Долгота.
        /// </summary>
        public double? Longitude { get; internal set; }

        /// <summary>
        /// Создает вложение геолокации.
        /// </summary>
        public static LocationAttachment Create(double latitude, double longitude) =>
            new LocationAttachment { Latitude = latitude, Longitude = longitude };
    }

    /// <summary>
    /// Вложение с inline клавиатурой.
    /// </summary>
    public class InlineKeyboardAttachment : IAttachment
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        public AttachmentType Type => AttachmentType.InlineKeyboard;

        /// <summary>
        /// Кнопки клавиатуры.
        /// </summary>
        public Button[][]? Buttons { get; internal set; }
    }
}
