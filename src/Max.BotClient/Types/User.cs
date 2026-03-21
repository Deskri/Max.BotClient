namespace Max.BotClient.Types
{
    /// <summary>
    /// Пользователь или бот.
    /// <see href="https://dev.max.ru/docs-api/objects/User"/>
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя или бота.
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Отображаемое имя пользователя или бота.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Отображаемая фамилия пользователя. Для ботов это поле не возвращается.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Публичный username пользователя. Может быть null, если недоступен.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Признак того, что аккаунт является ботом.
        /// </summary>
        public bool? IsBot { get; set; }

        /// <summary>
        /// Время последней активности пользователя (Unix timestamp в миллисекундах).
        /// </summary>
        public long? LastActivityTime { get; set; }

        /// <summary>
        /// Описание пользователя или бота.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// URL аватара пользователя или бота в уменьшенном размере.
        /// </summary>
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// URL аватара пользователя или бота в полном размере.
        /// </summary>
        public string? FullAvatarUrl { get; set; }
    }

    /// <summary>
    /// Пользователь с фотографией (расширенная информация).
    /// <see href="https://dev.max.ru/docs-api/objects/Chat"/>
    /// </summary>
    public class UserWithPhoto : User
    {
        // Наследует все поля от User
        // В базовом классе User уже есть Description, AvatarUrl, FullAvatarUrl
    }
}
