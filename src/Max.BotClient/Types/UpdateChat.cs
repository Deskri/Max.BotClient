namespace Max.BotClient.Types
{
    /// <summary>
    /// Параметры для обновления информации о чате.
    /// </summary>
    public class UpdateChatRequest
    {
        /// <summary>
        /// Иконка чата (изображение).
        /// </summary>
        public ChatIcon? Icon { get; set; }

        /// <summary>
        /// Название чата (от 1 до 200 символов).
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// ID сообщения для закрепления в чате. Для удаления закреплённого сообщения используйте метод Unpin.
        /// </summary>
        public string? Pin { get; set; }

        /// <summary>
        /// Если true, участники получат системное уведомление об изменении (по умолчанию true).
        /// </summary>
        public bool? Notify { get; set; }
    }

    /// <summary>
    /// Иконка чата (изображение).
    /// </summary>
    public class ChatIcon
    {
        /// <summary>
        /// URL внешнего изображения.
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

        /// <summary>
        /// Создать иконку с URL изображения.
        /// </summary>
        public static ChatIcon FromUrl(string url) => new ChatIcon { Url = url };

        /// <summary>
        /// Создать иконку с токеном изображения.
        /// </summary>
        public static ChatIcon FromToken(string token) => new ChatIcon { Token = token };

        /// <summary>
        /// Создать иконку с токеном загруженного изображения.
        /// </summary>
        public static ChatIcon FromPhotos(PhotoToken photos) => new ChatIcon { Photos = photos };
    }

    /// <summary>
    /// Токен загруженного изображения.
    /// </summary>
    public class PhotoToken
    {
        /// <summary>
        /// Закодированная информация загруженного изображения.
        /// </summary>
        public string? Token { get; set; }
    }
}
