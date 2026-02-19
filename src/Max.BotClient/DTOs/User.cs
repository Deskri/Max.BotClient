using System;

namespace Max.BotClient.DTOs
{
    /// <summary>
    /// Пользователь или бот.
    /// <see href="https://dev.max.ru/docs-api/objects/User"/>
    /// </summary>
    internal class User
    {
        /// <summary>
        /// Идентификатор пользователя или бота.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Отображаемое имя пользователя или бота.
        /// </summary>
        public string FirstName { get; set; }

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
        public bool IsBot { get; set; }

        /// <summary>
        /// Время последней активности пользователя (Unix timestamp в миллисекундах).
        /// Может отсутствовать из-за настроек приватности.
        /// </summary>
        public long? LastActivityTime { get; set; }

        /// <summary>
        /// Устаревшее поле, скоро будет удалено.
        /// </summary>
        [Obsolete("Deprecated field, will be removed soon")]
        public string? Name { get; set; }
    }

    /// <summary>
    /// Пользователь с фото.
    /// <see href="https://dev.max.ru/docs-api/objects/Chat"/>
    /// </summary>
    internal class UserWithPhoto : User
    {
        /// <summary>
        /// Описание пользователя или бота. Может быть null, если описание не заполнено.
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
    /// Информация о боте (ответ GET /me).
    /// <see href="https://dev.max.ru/docs-api/methods/GET/me"/>
    /// </summary>
    internal class UserBotInfo : UserWithPhoto
    {
        /// <summary>
        /// Команды бота (максимум 32).
        /// </summary>
        public BotCommandInfo[]? Commands { get; set; }
    }

    /// <summary>
    /// Команда бота.
    /// </summary>
    internal class BotCommandInfo
    {
        /// <summary>
        /// Название команды.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание команды.
        /// </summary>
        public string? Description { get; set; }
    }
}
