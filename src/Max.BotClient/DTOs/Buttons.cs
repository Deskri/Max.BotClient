namespace Max.BotClient.DTOs
{
    /// <summary>
    /// Клавиатура - двумерный массив кнопок.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class Keyboard
    {
        /// <summary>
        /// Двумерный массив кнопок.
        /// </summary>
        public IButton[][] Buttons { get; set; }
    }

    /// <summary>
    /// Тип кнопки.
    /// </summary>
    internal enum ButtonType
    {
        Callback,
        Link,
        RequestGeoLocation,
        RequestContact,
        OpenApp,
        Message
    }

    /// <summary>
    /// Интерфейс кнопки.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal interface IButton
    {
        /// <summary>
        /// Тип кнопки.
        /// </summary>
        ButtonType Type { get; }

        /// <summary>
        /// Видимый текст кнопки (1-128 символов).
        /// </summary>
        string Text { get; }
    }

    /// <summary>
    /// Кнопка с callback.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class CallbackButton : IButton
    {
        /// <inheritdoc />
        public ButtonType Type => ButtonType.Callback;

        /// <summary>
        /// Видимый текст кнопки (1-128 символов).
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Токен кнопки (до 1024 символов).
        /// </summary>
        public string Payload { get; set; }
    }

    /// <summary>
    /// Кнопка со ссылкой.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class LinkButton : IButton
    {
        /// <inheritdoc />
        public ButtonType Type => ButtonType.Link;

        /// <summary>
        /// Видимый текст кнопки (1-128 символов).
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// URL ссылки (до 2048 символов).
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// Кнопка запроса геолокации.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class RequestGeoLocationButton : IButton
    {
        /// <inheritdoc />
        public ButtonType Type => ButtonType.RequestGeoLocation;

        /// <summary>
        /// Видимый текст кнопки (1-128 символов).
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Если true, отправляет местоположение без запроса подтверждения пользователя.
        /// </summary>
        public bool? Quick { get; set; }
    }

    /// <summary>
    /// Кнопка запроса контакта.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class RequestContactButton : IButton
    {
        /// <inheritdoc />
        public ButtonType Type => ButtonType.RequestContact;

        /// <summary>
        /// Видимый текст кнопки (1-128 символов).
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Кнопка открытия мини-приложения.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class OpenAppButton : IButton
    {
        /// <inheritdoc />
        public ButtonType Type => ButtonType.OpenApp;

        /// <summary>
        /// Видимый текст кнопки (1-128 символов).
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Публичное имя (username) бота или ссылка на него, чьё мини-приложение надо запустить.
        /// </summary>
        public string? WebApp { get; set; }

        /// <summary>
        /// Идентификатор бота, чьё мини-приложение надо запустить.
        /// </summary>
        public long? ContactId { get; set; }

        /// <summary>
        /// Параметр запуска, который будет передан в initData мини-приложения.
        /// </summary>
        public string? Payload { get; set; }
    }

    /// <summary>
    /// Кнопка отправки сообщения.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    internal class MessageButton : IButton
    {
        /// <inheritdoc />
        public ButtonType Type => ButtonType.Message;

        /// <summary>
        /// Текст кнопки, который будет отправлен в чат от лица пользователя (1-128 символов).
        /// </summary>
        public string Text { get; set; }
    }
}
