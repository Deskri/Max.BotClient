namespace Max.BotClient.DTOs
{
    /// <summary>
    /// Тип чата.
    /// </summary>
    internal enum ChatType
    {
        Chat
    }

    /// <summary>
    /// Статус чата.
    /// </summary>
    internal enum ChatStatus
    {
        /// <summary>
        /// Бот является активным участником чата.
        /// </summary>
        Active,

        /// <summary>
        /// Бот был удалён из чата.
        /// </summary>
        Removed,

        /// <summary>
        /// Бот покинул чат.
        /// </summary>
        Left,

        /// <summary>
        /// Чат был закрыт.
        /// </summary>
        Closed
    }

    /// <summary>
    /// Чат.
    /// <see href="https://dev.max.ru/docs-api/objects/Chat"/>
    /// </summary>
    internal class Chat
    {
        /// <summary>
        /// ID чата.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Тип чата.
        /// </summary>
        public ChatType Type { get; set; }

        /// <summary>
        /// Статус чата.
        /// </summary>
        public ChatStatus Status { get; set; }

        /// <summary>
        /// Отображаемое название чата. Может быть null для диалогов.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Иконка чата.
        /// </summary>
        public Image? Icon { get; set; }

        /// <summary>
        /// Время последнего события в чате.
        /// </summary>
        public long LastEventTime { get; set; }

        /// <summary>
        /// Количество участников чата. Для диалогов всегда 2.
        /// </summary>
        public int ParticipantsCount { get; set; }

        /// <summary>
        /// ID владельца чата.
        /// </summary>
        public long? OwnerId { get; set; }

        /// <summary>
        /// Участники чата с временем последней активности. Может быть null, если запрашивается список чатов.
        /// </summary>
        public object? Participants { get; set; }

        /// <summary>
        /// Доступен ли чат публично (для диалогов всегда false).
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Ссылка на чат.
        /// </summary>
        public string? Link { get; set; }

        /// <summary>
        /// Описание чата.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Данные о пользователе в диалоге (только для чатов типа "dialog").
        /// </summary>
        public UserWithPhoto? DialogWithUser { get; set; }

        /// <summary>
        /// ID сообщения, содержащего кнопку, через которую был инициирован чат.
        /// </summary>
        public string? ChatMessageId { get; set; }

        /// <summary>
        /// Закреплённое сообщение в чате (возвращается только при запросе конкретного чата).
        /// </summary>
        public Message? PinnedMessage { get; set; }
    }

    /// <summary>
    /// Изображение.
    /// <see href="https://dev.max.ru/docs-api/objects/Chat"/>
    /// </summary>
    internal class Image
    {
        /// <summary>
        /// URL изображения.
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// Ответ на запрос списка чатов.
    /// </summary>
    internal class GetChatsResponse
    {
        /// <summary>
        /// Список запрашиваемых чатов.
        /// </summary>
        public Chat[]? Chats { get; set; }

        /// <summary>
        /// Указатель на следующую страницу запрашиваемых чатов.
        /// </summary>
        public long? Marker { get; set; }
    }

    /// <summary>
    /// Ответ на запрос закреплённого сообщения.
    /// </summary>
    internal class GetPinnedMessageResponse
    {
        /// <summary>
        /// Закреплённое сообщение. Может быть null, если в чате нет закреплённого сообщения.
        /// </summary>
        public Message? Message { get; set; }
    }

    /// <summary>
    /// Права администратора чата.
    /// </summary>
    internal enum ChatAdminPermission
    {
        ReadAllMessages,
        AddRemoveMembers,
        AddAdmins,
        ChangeChatInfo,
        PinMessage,
        Write,
        CanCall,
        EditLink,
        PostEditDeleteMessage,
        EditMessage,
        DeleteMessage
    }

    /// <summary>
    /// Участник чата с информацией о членстве.
    /// </summary>
    internal class ChatMember : UserWithPhoto
    {
        public long LastAccessTime { get; set; }
        public bool IsOwner { get; set; }
        public bool IsAdmin { get; set; }
        public long JoinTime { get; set; }
        public ChatAdminPermission[]? Permissions { get; set; }
        public string? Alias { get; set; }
    }

    /// <summary>
    /// Ответ на запрос списка участников чата.
    /// </summary>
    internal class GetChatMembersResponse
    {
        public ChatMember[]? Members { get; set; }
        public long? Marker { get; set; }
    }

    /// <summary>
    /// Администратор чата для запроса назначения.
    /// </summary>
    internal class ChatAdmin
    {
        public long UserId { get; set; }
        public ChatAdminPermission[]? Permissions { get; set; }
        public string? Alias { get; set; }
    }

    /// <summary>
    /// Запрос на добавление администраторов.
    /// </summary>
    internal class AddChatAdminsRequest
    {
        public ChatAdmin[]? Admins { get; set; }
    }
}
