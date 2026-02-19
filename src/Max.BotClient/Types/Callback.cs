namespace Max.BotClient.Types
{
    /// <summary>
    /// Данные callback.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    public class Callback
    {
        /// <summary>
        /// Unix-время, когда пользователь нажал кнопку.
        /// </summary>
        public long? Timestamp { get; set; }

        /// <summary>
        /// Текущий ID клавиатуры.
        /// </summary>
        public string? CallbackId { get; set; }

        /// <summary>
        /// Токен кнопки.
        /// </summary>
        public string? Payload { get; set; }

        /// <summary>
        /// Пользователь, нажавший на кнопку.
        /// </summary>
        public User? User { get; set; }
    }
}
