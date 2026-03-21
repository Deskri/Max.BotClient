namespace Max.BotClient.DTOs
{
    /// <summary>
    /// Тип обновления.
    /// </summary>
    internal enum UpdateType
    {
        MessageCreated,
        MessageCallback,
        MessageEdited,
        MessageRemoved,
        BotAdded,
        BotRemoved,
        DialogMuted,
        DialogUnmuted,
        DialogCleared,
        DialogRemoved,
        UserAdded,
        UserRemoved,
        BotStarted,
        BotStopped,
        ChatTitleChanged
    }

    /// <summary>
    /// Интерфейс обновления.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal interface IUpdate
    {
        /// <summary>
        /// Тип обновления.
        /// </summary>
        UpdateType UpdateType { get; }

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        long Timestamp { get; }
    }

    /// <summary>
    /// Данные callback.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class Callback
    {
        /// <summary>
        /// Unix-время, когда пользователь нажал кнопку.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Текущий ID клавиатуры.
        /// </summary>
        public string CallbackId { get; set; }

        /// <summary>
        /// Токен кнопки.
        /// </summary>
        public string? Payload { get; set; }

        /// <summary>
        /// Пользователь, нажавший на кнопку.
        /// </summary>
        public User User { get; set; }
    }

    /// <summary>
    /// Обновление о создании нового сообщения.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class MessageCreatedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.MessageCreated;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Новое созданное сообщение.
        /// </summary>
        public Message Message { get; set; }

        /// <summary>
        /// Текущий язык пользователя в формате IETF BCP 47. Доступно только в диалогах.
        /// </summary>
        public string? UserLocale { get; set; }
    }

    /// <summary>
    /// Обновление о нажатии на callback-кнопку.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class MessageCallbackUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.MessageCallback;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Данные callback.
        /// </summary>
        public Callback Callback { get; set; }

        /// <summary>
        /// Изначальное сообщение, содержащее встроенную клавиатуру. Может быть null, если оно было удалено.
        /// </summary>
        public Message? Message { get; set; }

        /// <summary>
        /// Текущий язык пользователя в формате IETF BCP 47.
        /// </summary>
        public string? UserLocale { get; set; }
    }

    /// <summary>
    /// Обновление о редактировании сообщения.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class MessageEditedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.MessageEdited;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Отредактированное сообщение.
        /// </summary>
        public Message Message { get; set; }
    }

    /// <summary>
    /// Обновление об удалении сообщения.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class MessageRemovedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.MessageRemoved;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// ID удаленного сообщения.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// ID чата, где сообщение было удалено.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Пользователь, удаливший сообщение.
        /// </summary>
        public long UserId { get; set; }
    }

    /// <summary>
    /// Обновление о добавлении бота в чат.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class BotAddedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.BotAdded;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// ID чата, куда был добавлен бот.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Пользователь, добавивший бота в чат.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Указывает, был ли бот добавлен в канал или нет.
        /// </summary>
        public bool IsChannel { get; set; }
    }

    /// <summary>
    /// Обновление об удалении бота из чата.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class BotRemovedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.BotRemoved;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// ID чата, откуда был удалён бот.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Пользователь, удаливший бота из чата.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Указывает, был ли бот удалён из канала или нет.
        /// </summary>
        public bool IsChannel { get; set; }
    }

    /// <summary>
    /// Обновление об отключении уведомлений в диалоге.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class DialogMutedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.DialogMuted;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// ID чата, где произошло событие.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Пользователь, который отключил уведомления.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Время в формате Unix, до наступления которого диалог был отключён.
        /// </summary>
        public long MutedUntil { get; set; }

        /// <summary>
        /// Текущий язык пользователя в формате IETF BCP 47.
        /// </summary>
        public string? UserLocale { get; set; }
    }

    /// <summary>
    /// Обновление о включении уведомлений в диалоге.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class DialogUnmutedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.DialogUnmuted;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// ID чата, где произошло событие.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Пользователь, который включил уведомления.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Текущий язык пользователя в формате IETF BCP 47.
        /// </summary>
        public string? UserLocale { get; set; }
    }

    /// <summary>
    /// Обновление об очистке диалога.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class DialogClearedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.DialogCleared;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// ID чата, где произошло событие.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Пользователь, который очистил диалог.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Текущий язык пользователя в формате IETF BCP 47.
        /// </summary>
        public string? UserLocale { get; set; }
    }

    /// <summary>
    /// Обновление об удалении диалога.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class DialogRemovedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.DialogRemoved;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// ID чата, где произошло событие.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Пользователь, который удалил чат.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Текущий язык пользователя в формате IETF BCP 47.
        /// </summary>
        public string? UserLocale { get; set; }
    }

    /// <summary>
    /// Обновление о добавлении пользователя в чат.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class UserAddedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.UserAdded;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// ID чата, где произошло событие.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Пользователь, добавленный в чат.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Пользователь, который добавил пользователя в чат. Может быть null, если пользователь присоединился по ссылке.
        /// </summary>
        public long? InviterId { get; set; }

        /// <summary>
        /// Указывает, был ли пользователь добавлен в канал или нет.
        /// </summary>
        public bool IsChannel { get; set; }
    }

    /// <summary>
    /// Обновление об удалении пользователя из чата.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class UserRemovedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.UserRemoved;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// ID чата, где произошло событие.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Пользователь, удалённый из чата.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Администратор, который удалил пользователя из чата. Может быть null, если пользователь покинул чат сам.
        /// </summary>
        public long? AdminId { get; set; }

        /// <summary>
        /// Указывает, был ли пользователь удалён из канала или нет.
        /// </summary>
        public bool IsChannel { get; set; }
    }

    /// <summary>
    /// Обновление о запуске бота пользователем.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class BotStartedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.BotStarted;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// ID диалога, где произошло событие.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Пользователь, который нажал кнопку 'Start'.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Дополнительные данные из дип-линков, переданные при запуске бота (до 512 символов).
        /// </summary>
        public string? Payload { get; set; }

        /// <summary>
        /// Текущий язык пользователя в формате IETF BCP 47.
        /// </summary>
        public string? UserLocale { get; set; }
    }

    /// <summary>
    /// Обновление об остановке бота пользователем.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class BotStoppedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.BotStopped;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// ID диалога, где произошло событие.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Пользователь, который остановил бота.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Текущий язык пользователя в формате IETF BCP 47.
        /// </summary>
        public string? UserLocale { get; set; }
    }

    /// <summary>
    /// Обновление об изменении названия чата.
    /// <see href="https://dev.max.ru/docs-api/objects/Update"/>
    /// </summary>
    internal class ChatTitleChangedUpdate : IUpdate
    {
        /// <inheritdoc />
        public UpdateType UpdateType => UpdateType.ChatTitleChanged;

        /// <summary>
        /// Unix-время, когда произошло событие.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// ID чата, где произошло событие.
        /// </summary>
        public long ChatId { get; set; }

        /// <summary>
        /// Пользователь, который изменил название.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Новое название.
        /// </summary>
        public string Title { get; set; }
    }

    /// <summary>
    /// Ответ с обновлениями.
    /// <see href="https://dev.max.ru/docs-api/methods/GET/updates"/>
    /// </summary>
    internal class GetUpdatesResponse
    {
        /// <summary>
        /// Список обновлений.
        /// </summary>
        public IUpdate[] Updates { get; set; } = [];

        /// <summary>
        /// Маркер для следующего запроса.
        /// </summary>
        public long? Marker { get; set; }
    }
}
