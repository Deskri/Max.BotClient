namespace Max.BotClient.Types
{
    /// <summary>
    /// Статус чата.
    /// </summary>
    public enum ChatStatus
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
    /// Права администратора чата.
    /// </summary>
    public enum ChatAdminPermission
    {
        /// <summary>
        /// Читать все сообщения.
        /// </summary>
        ReadAllMessages,

        /// <summary>
        /// Добавлять/удалять участников.
        /// </summary>
        AddRemoveMembers,

        /// <summary>
        /// Добавлять администраторов.
        /// </summary>
        AddAdmins,

        /// <summary>
        /// Изменять информацию о чате.
        /// </summary>
        ChangeChatInfo,

        /// <summary>
        /// Закреплять сообщения.
        /// </summary>
        PinMessage,

        /// <summary>
        /// Писать сообщения.
        /// </summary>
        Write,

        /// <summary>
        /// Совершать звонки.
        /// </summary>
        CanCall,

        /// <summary>
        /// Изменять ссылку на чат.
        /// </summary>
        EditLink,

        /// <summary>
        /// Публиковать, редактировать и удалять сообщения.
        /// </summary>
        PostEditDeleteMessage,

        /// <summary>
        /// Редактировать сообщения.
        /// </summary>
        EditMessage,

        /// <summary>
        /// Удалять сообщения.
        /// </summary>
        DeleteMessage
    }

    /// <summary>
    /// Действие бота в чате.
    /// <see href="https://dev.max.ru/docs-api/methods/POST/chats/-chatId-/actions"/>
    /// </summary>
    public enum SenderAction
    {
        /// <summary>
        /// Бот набирает сообщение.
        /// </summary>
        TypingOn,

        /// <summary>
        /// Бот отправляет фото.
        /// </summary>
        SendingPhoto,

        /// <summary>
        /// Бот отправляет видео.
        /// </summary>
        SendingVideo,

        /// <summary>
        /// Бот отправляет аудиофайл.
        /// </summary>
        SendingAudio,

        /// <summary>
        /// Бот отправляет файл.
        /// </summary>
        SendingFile,

        /// <summary>
        /// Бот помечает сообщения как прочитанные.
        /// </summary>
        MarkSeen
    }

    /// <summary>
    /// Изображение.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// URL изображения.
        /// </summary>
        public string? Url { get; internal set; }
    }

    /// <summary>
    /// Чат.
    /// <see href="https://dev.max.ru/docs-api/objects/Chat"/>
    /// </summary>
    public class Chat
    {
        /// <summary>
        /// ID чата.
        /// </summary>
        public long ChatId { get; internal set; }

        /// <summary>
        /// Тип чата.
        /// </summary>
        public ChatType Type { get; internal set; }

        /// <summary>
        /// Статус чата.
        /// </summary>
        public ChatStatus Status { get; internal set; }

        /// <summary>
        /// Отображаемое название чата. Может быть null для диалогов.
        /// </summary>
        public string? Title { get; internal set; }

        /// <summary>
        /// Иконка чата.
        /// </summary>
        public Image? Icon { get; internal set; }

        /// <summary>
        /// Время последнего события в чате (Unix-время в миллисекундах).
        /// </summary>
        public long LastEventTime { get; internal set; }

        /// <summary>
        /// Количество участников чата. Для диалогов всегда 2.
        /// </summary>
        public int ParticipantsCount { get; internal set; }

        /// <summary>
        /// ID владельца чата.
        /// </summary>
        public long? OwnerId { get; internal set; }

        /// <summary>
        /// Участники чата с временем последней активности. Может быть null, если запрашивается список чатов.
        /// </summary>
        public object? Participants { get; internal set; }

        /// <summary>
        /// Доступен ли чат публично (для диалогов всегда false).
        /// </summary>
        public bool IsPublic { get; internal set; }

        /// <summary>
        /// Ссылка на чат.
        /// </summary>
        public string? Link { get; internal set; }

        /// <summary>
        /// Описание чата.
        /// </summary>
        public string? Description { get; internal set; }

        /// <summary>
        /// Данные о пользователе в диалоге (только для чатов типа "dialog").
        /// </summary>
        public UserWithPhoto? DialogWithUser { get; internal set; }

        /// <summary>
        /// ID сообщения, содержащего кнопку, через которую был инициирован чат.
        /// </summary>
        public string? ChatMessageId { get; internal set; }

        /// <summary>
        /// Закреплённое сообщение в чате (возвращается только при запросе конкретного чата).
        /// </summary>
        public Message? PinnedMessage { get; internal set; }
    }

    /// <summary>
    /// Ответ на запрос списка чатов.
    /// </summary>
    public class GetChatsResponse
    {
        /// <summary>
        /// Список запрашиваемых чатов.
        /// </summary>
        public Chat[]? Chats { get; internal set; }

        /// <summary>
        /// Указатель на следующую страницу запрашиваемых чатов.
        /// </summary>
        public long? Marker { get; internal set; }
    }

    /// <summary>
    /// Ответ на запрос закреплённого сообщения.
    /// </summary>
    public class GetPinnedMessageResponse
    {
        /// <summary>
        /// Закреплённое сообщение. Может быть null, если в чате нет закреплённого сообщения.
        /// </summary>
        public Message? Message { get; internal set; }
    }

    /// <summary>
    /// Участник чата с информацией о членстве.
    /// <see href="https://dev.max.ru/docs-api/methods/GET/chats/-chatId-/members/me"/>
    /// </summary>
    public class ChatMember : UserWithPhoto
    {
        /// <summary>
        /// Время последней активности пользователя в чате (Unix-время в миллисекундах).
        /// </summary>
        public long LastAccessTime { get; internal set; }

        /// <summary>
        /// Является ли пользователь владельцем чата.
        /// </summary>
        public bool IsOwner { get; internal set; }

        /// <summary>
        /// Является ли пользователь администратором чата.
        /// </summary>
        public bool IsAdmin { get; internal set; }

        /// <summary>
        /// Дата присоединения к чату (Unix-время в миллисекундах).
        /// </summary>
        public long JoinTime { get; internal set; }

        /// <summary>
        /// Перечень прав пользователя в чате.
        /// </summary>
        public ChatAdminPermission[]? Permissions { get; internal set; }

        /// <summary>
        /// Заголовок, который будет показан на клиенте (например, "владелец", "админ").
        /// </summary>
        public string? Alias { get; internal set; }
    }

    /// <summary>
    /// Ответ на запрос списка участников чата.
    /// </summary>
    public class GetChatMembersResponse
    {
        /// <summary>
        /// Список участников чата.
        /// </summary>
        public ChatMember[]? Members { get; internal set; }

        /// <summary>
        /// Указатель на следующую страницу данных.
        /// </summary>
        public long? Marker { get; internal set; }
    }

    /// <summary>
    /// Администратор чата для назначения.
    /// <see href="https://dev.max.ru/docs-api/methods/POST/chats/-chatId-/members/admins"/>
    /// </summary>
    public class ChatAdmin
    {
        /// <summary>
        /// Идентификатор пользователя-участника чата. Максимум 50 администраторов в чате.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Перечень прав доступа. Право "ReadAllMessages" важно для ботов — без него бот не получает апдейты.
        /// </summary>
        public ChatAdminPermission[]? Permissions { get; set; }

        /// <summary>
        /// Заголовок, который будет показан на клиенте (например, "модератор").
        /// </summary>
        public string? Alias { get; set; }

        /// <summary>
        /// Создать администратора с указанными правами.
        /// </summary>
        public ChatAdmin(long userId, params ChatAdminPermission[] permissions)
        {
            UserId = userId;
            Permissions = permissions;
        }

        /// <summary>
        /// Создать администратора с указанными правами и заголовком.
        /// </summary>
        public ChatAdmin(long userId, string alias, params ChatAdminPermission[] permissions)
        {
            UserId = userId;
            Alias = alias;
            Permissions = permissions;
        }
    }
}
