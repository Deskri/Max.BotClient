namespace Max.BotClient.Mapping
{
    /// <summary>
    /// Методы расширения для маппинга UpdateChat запросов между Types и DTOs.
    /// </summary>
    internal static class UpdateChatMappingExtensions
    {
        /// <summary>
        /// Преобразует Types UpdateChatRequest в DTO.
        /// </summary>
        public static object? ToDto(this Types.UpdateChatRequest? request)
        {
            if (request == null) return null;

            return new
            {
                icon = request.Icon?.ToChatIconDto(),
                title = request.Title,
                pin = request.Pin,
                notify = request.Notify
            };
        }

        /// <summary>
        /// Преобразует Types ChatIcon в DTO PhotoAttachmentRequestPayload.
        /// </summary>
        private static DTOs.PhotoAttachmentRequestPayload? ToChatIconDto(this Types.ChatIcon? icon)
        {
            if (icon == null) return null;

            return new DTOs.PhotoAttachmentRequestPayload
            {
                Url = icon.Url,
                Token = icon.Token,
                Photos = icon.Photos?.ToPhotoTokenDto()
            };
        }

        /// <summary>
        /// Преобразует Types PhotoToken в DTO PhotoToken.
        /// </summary>
        private static DTOs.PhotoToken? ToPhotoTokenDto(this Types.PhotoToken? photoToken)
        {
            if (photoToken == null) return null;

            return new DTOs.PhotoToken
            {
                Token = photoToken.Token
            };
        }
    }
}
