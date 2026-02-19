namespace Max.BotClient.Mapping
{
    /// <summary>
    /// Методы расширения для маппинга типов Video между DTOs и Types.
    /// </summary>
    internal static class VideoMappingExtensions
    {
        /// <summary>
        /// Преобразует DTO VideoInfo в Types VideoInfo.
        /// </summary>
        public static Types.VideoInfo? ToVideoInfo(this DTOs.VideoInfo? dto)
        {
            if (dto == null) return null;

            return new Types.VideoInfo
            {
                Token = dto.Token,
                Urls = dto.Urls?.ToVideoUrls(),
                Thumbnail = dto.Thumbnail != null ? new Types.PhotoAttachment
                {
                    Url = dto.Thumbnail.Url,
                    Token = dto.Thumbnail.Token,
                    PhotoId = dto.Thumbnail.PhotoId
                } : null,
                Width = dto.Width,
                Height = dto.Height,
                Duration = dto.Duration
            };
        }

        /// <summary>
        /// Преобразует DTO VideoUrls в Types VideoUrls.
        /// </summary>
        public static Types.VideoUrls? ToVideoUrls(this DTOs.VideoUrls? dto)
        {
            if (dto == null) return null;

            return new Types.VideoUrls
            {
                Mp4Resolution1080p = dto.Mp4Resolution1080p,
                Mp4Resolution720p = dto.Mp4Resolution720p,
                Mp4Resolution480p = dto.Mp4Resolution480p,
                Mp4Resolution360p = dto.Mp4Resolution360p,
                Mp4Resolution240p = dto.Mp4Resolution240p,
                Mp4Resolution144p = dto.Mp4Resolution144p,
                HlsStream = dto.HlsStream
            };
        }
    }
}
