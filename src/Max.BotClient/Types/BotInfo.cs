namespace Max.BotClient.Types
{
    /// <summary>
    /// Информация о боте.
    /// <see href="https://dev.max.ru/docs-api/methods/GET/me"/>
    /// </summary>
    public class BotInfo
    {
        public long UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public bool IsBot { get; set; }
        public long? LastActivityTime { get; set; }
        public string? Description { get; set; }
        public string? AvatarUrl { get; set; }
        public string? FullAvatarUrl { get; set; }
        public BotCommand[]? Commands { get; set; }
    }

    /// <summary>
    /// Команда бота.
    /// </summary>
    public class BotCommand
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
