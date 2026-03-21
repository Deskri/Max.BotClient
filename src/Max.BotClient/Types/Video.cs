namespace Max.BotClient.Types
{
    /// <summary>
    /// URL-адреса для скачивания или воспроизведения видео.
    /// </summary>
    public class VideoUrls
    {
        /// <summary>
        /// URL видео в формате MP4 с разрешением 1080p, если доступно.
        /// </summary>
        public string? Mp4Resolution1080p { get; internal set; }

        /// <summary>
        /// URL видео в формате MP4 с разрешением 720p, если доступно.
        /// </summary>
        public string? Mp4Resolution720p { get; internal set; }

        /// <summary>
        /// URL видео в формате MP4 с разрешением 480p, если доступно.
        /// </summary>
        public string? Mp4Resolution480p { get; internal set; }

        /// <summary>
        /// URL видео в формате MP4 с разрешением 360p, если доступно.
        /// </summary>
        public string? Mp4Resolution360p { get; internal set; }

        /// <summary>
        /// URL видео в формате MP4 с разрешением 240p, если доступно.
        /// </summary>
        public string? Mp4Resolution240p { get; internal set; }

        /// <summary>
        /// URL видео в формате MP4 с разрешением 144p, если доступно.
        /// </summary>
        public string? Mp4Resolution144p { get; internal set; }

        /// <summary>
        /// URL потоковой трансляции в формате HLS, если доступна.
        /// </summary>
        public string? HlsStream { get; internal set; }
    }

    /// <summary>
    /// Подробная информация о прикреплённом видео.
    /// <see href="https://dev.max.ru/docs-api/methods/GET/videos/-videoToken-"/>
    /// </summary>
    public class VideoInfo
    {
        /// <summary>
        /// Токен видео-вложения.
        /// </summary>
        public string? Token { get; internal set; }

        /// <summary>
        /// URL-адреса для скачивания или воспроизведения видео. Может быть null, если видео недоступно.
        /// </summary>
        public VideoUrls? Urls { get; internal set; }

        /// <summary>
        /// Миниатюра видео.
        /// </summary>
        public PhotoAttachment? Thumbnail { get; internal set; }

        /// <summary>
        /// Ширина видео.
        /// </summary>
        public int? Width { get; internal set; }

        /// <summary>
        /// Высота видео.
        /// </summary>
        public int? Height { get; internal set; }

        /// <summary>
        /// Длина видео в секундах.
        /// </summary>
        public int? Duration { get; internal set; }
    }
}
