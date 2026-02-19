using System.Text.Json.Serialization;

namespace Max.BotClient.DTOs
{
    /// <summary>
    /// URL-адреса для скачивания или воспроизведения видео.
    /// </summary>
    internal class VideoUrls
    {
        /// <summary>
        /// URL видео в формате MP4 с разрешением 1080p, если доступно.
        /// </summary>
        [JsonPropertyName("mp4_1080")]
        public string? Mp4Resolution1080p { get; set; }

        /// <summary>
        /// URL видео в формате MP4 с разрешением 720p, если доступно.
        /// </summary>
        [JsonPropertyName("mp4_720")]
        public string? Mp4Resolution720p { get; set; }

        /// <summary>
        /// URL видео в формате MP4 с разрешением 480p, если доступно.
        /// </summary>
        [JsonPropertyName("mp4_480")]
        public string? Mp4Resolution480p { get; set; }

        /// <summary>
        /// URL видео в формате MP4 с разрешением 360p, если доступно.
        /// </summary>
        [JsonPropertyName("mp4_360")]
        public string? Mp4Resolution360p { get; set; }

        /// <summary>
        /// URL видео в формате MP4 с разрешением 240p, если доступно.
        /// </summary>
        [JsonPropertyName("mp4_240")]
        public string? Mp4Resolution240p { get; set; }

        /// <summary>
        /// URL видео в формате MP4 с разрешением 144p, если доступно.
        /// </summary>
        [JsonPropertyName("mp4_144")]
        public string? Mp4Resolution144p { get; set; }

        /// <summary>
        /// URL потоковой трансляции в формате HLS, если доступна.
        /// </summary>
        [JsonPropertyName("hls")]
        public string? HlsStream { get; set; }
    }

    /// <summary>
    /// Подробная информация о прикреплённом видео (DTO).
    /// </summary>
    internal class VideoInfo
    {
        /// <summary>
        /// Токен видео-вложения.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// URL-адреса для скачивания или воспроизведения видео.
        /// </summary>
        public VideoUrls? Urls { get; set; }

        /// <summary>
        /// Миниатюра видео.
        /// </summary>
        public PhotoAttachmentPayload? Thumbnail { get; set; }

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
}
