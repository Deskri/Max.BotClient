namespace Max.BotClient.Types
{
    /// <summary>
    /// Тип загружаемого файла.
    /// </summary>
    public enum UploadType
    {
        Image,
        Video,
        Audio,
        File
    }

    /// <summary>
    /// Результат запроса URL для загрузки файла.
    /// <see href="https://dev.max.ru/docs-api/methods/POST/uploads"/>
    /// </summary>
    public class UploadResult
    {
        /// <summary>
        /// URL для загрузки файла.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Токен для video/audio (используется при отправке сообщения).
        /// </summary>
        public string? Token { get; set; }
    }
}
