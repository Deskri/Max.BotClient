using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Max.BotClient.Mapping;

namespace Max.BotClient
{
    public static partial class BotClientApiMethods
    {
        /// <summary>
        /// Получить сообщения из чата.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/messages"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="from">Время начала (Unix timestamp).</param>
        /// <param name="to">Время окончания (Unix timestamp).</param>
        /// <param name="count">Максимальное количество сообщений (1-100, по умолчанию 50).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Массив сообщений (последние сообщения первыми).</returns>
        public static async Task<Types.Message[]> GetMessages(
            this BotClient botClient,
            long chatId,
            long? from = null,
            long? to = null,
            int? count = null,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<GetMessagesByChatParams, DTOs.GetMessagesResponse, Types.GetMessagesResponse>(
            HttpMethod.Get,
            "/messages",
            () => new GetMessagesByChatParams { ChatId = chatId, From = from, To = to, Count = count },
            cancellationToken
        )).Messages ?? Array.Empty<Types.Message>();

        /// <summary>
        /// Получить сообщения по их ID.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/messages"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="messageIds">Список ID сообщений.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Массив сообщений.</returns>
        public static async Task<Types.Message[]> GetMessages(
            this BotClient botClient,
            string[] messageIds,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<GetMessagesByIdsParams, DTOs.GetMessagesResponse, Types.GetMessagesResponse>(
            HttpMethod.Get,
            "/messages",
            () => new GetMessagesByIdsParams { MessageIds = messageIds },
            cancellationToken
        )).Messages ?? Array.Empty<Types.Message>();

        /// <summary>
        /// Получить сообщение по его ID.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/messages/-messageId-"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="messageId">ID сообщения (mid).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Сообщение с указанным ID.</returns>
        public static async Task<Types.Message> GetMessage(
            this BotClient botClient,
            string messageId,
            CancellationToken cancellationToken = default
        ) => await botClient.ProcessApi<DTOs.Message, Types.Message>(
            HttpMethod.Get,
            $"/messages/{Uri.EscapeDataString(messageId)}",
            cancellationToken: cancellationToken
        );

        /// <summary>
        /// Отправить сообщение пользователю или в чат.
        /// <see href="https://dev.max.ru/docs-api/methods/POST/messages"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="id">ID получателя (пользователя или чата).</param>
        /// <param name="message">Сообщение для отправки. Используйте .ToChat() для отправки в чат.</param>
        /// <param name="disableLinkPreview">Отключить превью ссылок.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Отправленное сообщение.</returns>
        public static async Task<Types.Message?> SendMessage(
            this BotClient botClient,
            long id,
            Types.Message message,
            bool? disableLinkPreview = null,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<SendMessageParams, DTOs.SendMessageResponse, Types.SendMessageResponse>(
            HttpMethod.Post,
            "/messages",
            () => new SendMessageParams
            {
                ChatId = message.GetRecipientType() == Types.RecipientType.Chat ? id : null,
                UserId = message.GetRecipientType() != Types.RecipientType.Chat ? id : null,
                DisableLinkPreview = disableLinkPreview,
                Body = message.ToMessageBody().ToDto()
            },
            cancellationToken
        )).Message;

        /// <summary>
        /// Редактировать сообщение в чате.
        /// <see href="https://dev.max.ru/docs-api/methods/PUT/messages"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="messageId">ID редактируемого сообщения.</param>
        /// <param name="message">Сообщение с измененными данными. Все вложения указанного типа будут заменены.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>true, если запрос был успешным.</returns>
        /// <remarks>С помощью метода можно отредактировать сообщения, которые отправлены менее 24 часов назад.</remarks>
        public static async Task<bool> EditMessage(
            this BotClient botClient,
            string messageId,
            Types.Message message,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<EditMessageParams, DTOs.ApiResponse>(
            new HttpMethod("PUT"),
            "/messages",
            () => new EditMessageParams { MessageId = messageId, Body = message.ToMessageBody().ToDto() },
            cancellationToken
        )).Success;

        /// <summary>
        /// Удалить сообщение в диалоге или чате.
        /// <see href="https://dev.max.ru/docs-api/methods/DELETE/messages"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="messageId">ID удаляемого сообщения.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>true, если запрос был успешным.</returns>
        /// <remarks>С помощью метода можно удалять сообщения, которые отправлены менее 24 часов назад. Бот должен иметь разрешение на удаление сообщений.</remarks>
        public static async Task<bool> DeleteMessage(
            this BotClient botClient,
            string messageId,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<DeleteMessageParams, DTOs.ApiResponse>(
            HttpMethod.Delete,
            "/messages",
            () => new DeleteMessageParams { MessageId = messageId },
            cancellationToken
        )).Success;

        /// <summary>
        /// Отправить ответ на нажатие кнопки пользователем.
        /// <see href="https://dev.max.ru/docs-api/methods/POST/answers"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="callbackId">Идентификатор кнопки (получен из update с типом message_callback).</param>
        /// <param name="message">Новое сообщение для обновления текущего (необязательно).</param>
        /// <param name="notification">Текст одноразового уведомления для пользователя (необязательно).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>true, если запрос был успешным.</returns>
        /// <remarks>Хотя бы один из параметров message или notification должен быть указан.</remarks>
        public static async Task<bool> AnswerCallback(
            this BotClient botClient,
            string callbackId,
            Types.Message? message = null,
            string? notification = null,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<AnswerCallbackParams, DTOs.ApiResponse>(
            HttpMethod.Post,
            "/answers",
            () => new AnswerCallbackParams
            {
                CallbackId = callbackId,
                Message = message?.ToMessageBody().ToDto(),
                Notification = notification
            },
            cancellationToken
        )).Success;
    }
}
