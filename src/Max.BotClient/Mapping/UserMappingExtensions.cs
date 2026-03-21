namespace Max.BotClient.Mapping
{
    /// <summary>
    /// Методы расширения для маппинга типов User между DTOs и Types.
    /// </summary>
    internal static class UserMappingExtensions
    {
        /// <summary>
        /// Преобразует DTO User в Types User.
        /// </summary>
        public static Types.User? ToUser(this DTOs.User? dto)
        {
            if (dto == null) return null;

            return new Types.User
            {
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                IsBot = dto.IsBot,
                LastActivityTime = dto.LastActivityTime
            };
        }

        /// <summary>
        /// Преобразует DTO UserWithPhoto в Types UserWithPhoto.
        /// </summary>
        public static Types.UserWithPhoto? ToUserWithPhoto(this DTOs.UserWithPhoto? dto)
        {
            if (dto == null) return null;

            return new Types.UserWithPhoto
            {
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                IsBot = dto.IsBot,
                LastActivityTime = dto.LastActivityTime,
                Description = dto.Description,
                AvatarUrl = dto.AvatarUrl,
                FullAvatarUrl = dto.FullAvatarUrl
            };
        }
    }
}
