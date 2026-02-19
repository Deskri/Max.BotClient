using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Max.BotClient.DTOs;
using Max.BotClient.Mapping;

namespace Max.BotClient
{
    public static partial class BotClientApiMethods
    {
        /// <summary>
        /// Получить список групповых чатов, в которых участвовал бот.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/chats"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="count">Количество запрашиваемых чатов (1-100, по умолчанию 50).</param>
        /// <param name="marker">Указатель на следующую страницу данных. Для первой страницы передайте null.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список чатов и маркер для следующей страницы.</returns>
        public static async Task<Types.GetChatsResponse> GetChats(
            this BotClient botClient,
            int? count = null,
            long? marker = null,
            CancellationToken cancellationToken = default
        ) => await botClient.ProcessApi<GetChatsParams, DTOs.GetChatsResponse, Types.GetChatsResponse>(
            HttpMethod.Get,
            "/chats",
            () => new GetChatsParams { Count = count, Marker = marker },
            cancellationToken
        );

        /// <summary>
        /// Получить информацию о групповом чате по его ID.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/chats/-chatId-"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID запрашиваемого чата.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Информация о чате, включая закреплённое сообщение (если есть).</returns>
        public static async Task<Types.Chat> GetChat(
            this BotClient botClient,
            long chatId,
            CancellationToken cancellationToken = default
        ) => await botClient.ProcessApi<DTOs.Chat, Types.Chat>(
            HttpMethod.Get,
            $"/chats/{chatId}",
            cancellationToken: cancellationToken
        );

        /// <summary>
        /// Изменить информацию о групповом чате.
        /// <see href="https://dev.max.ru/docs-api/methods/PATCH/chats/-chatId-"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="request">Параметры для обновления чата.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Обновлённая информация о чате.</returns>
        public static async Task<Types.Chat> UpdateChat(
            this BotClient botClient,
            long chatId,
            Types.UpdateChatRequest request,
            CancellationToken cancellationToken = default
        ) => await botClient.ProcessApi<DTOs.Chat, Types.Chat>(
            new HttpMethod("PATCH"),
            $"/chats/{chatId}",
            request.ToDto(),
            cancellationToken
        );

        /// <summary>
        /// Удалить групповой чат для всех участников.
        /// <see href="https://dev.max.ru/docs-api/methods/DELETE/chats/-chatId-"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>true, если запрос был успешным.</returns>
        public static async Task<bool> DeleteChat(
            this BotClient botClient,
            long chatId,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<ApiResponse>(
            HttpMethod.Delete,
            $"/chats/{chatId}",
            cancellationToken: cancellationToken
        )).Success;

        /// <summary>
        /// Отправить действие бота в групповой чат.
        /// <see href="https://dev.max.ru/docs-api/methods/POST/chats/-chatId-/actions"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="action">Действие бота (например: набор текста, отправка фото).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>true, если запрос был успешным.</returns>
        public static async Task<bool> SendAction(
            this BotClient botClient,
            long chatId,
            Types.SenderAction action,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<SendActionParams, ApiResponse>(
            HttpMethod.Post,
            $"/chats/{chatId}/actions",
            () => new SendActionParams { Action = action },
            cancellationToken
        )).Success;

        /// <summary>
        /// Получить закреплённое сообщение в групповом чате.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/chats/-chatId-/pin"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Закреплённое сообщение или null, если в чате нет закреплённого сообщения.</returns>
        public static async Task<Types.Message?> GetPinnedMessage(
            this BotClient botClient,
            long chatId,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<DTOs.GetPinnedMessageResponse, Types.GetPinnedMessageResponse>(
            HttpMethod.Get,
            $"/chats/{chatId}/pin",
            cancellationToken: cancellationToken
        )).Message;

        /// <summary>
        /// Закрепить сообщение в групповом чате.
        /// <see href="https://dev.max.ru/docs-api/methods/PUT/chats/-chatId-/pin"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="messageId">ID сообщения для закрепления (Message.Mid).</param>
        /// <param name="notify">Если true, участники получат уведомление о закреплении. По умолчанию true.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>true, если запрос был успешным.</returns>
        public static async Task<bool> PinMessage(
            this BotClient botClient,
            long chatId,
            string messageId,
            bool? notify = null,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<PinMessageParams, ApiResponse>(
            new HttpMethod("PUT"),
            $"/chats/{chatId}/pin",
            () => new PinMessageParams { MessageId = messageId, Notify = notify },
            cancellationToken
        )).Success;

        /// <summary>
        /// Удалить закреплённое сообщение в групповом чате.
        /// <see href="https://dev.max.ru/docs-api/methods/DELETE/chats/-chatId-/pin"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>true, если запрос был успешным.</returns>
        public static async Task<bool> UnpinMessage(
            this BotClient botClient,
            long chatId,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<ApiResponse>(
            HttpMethod.Delete,
            $"/chats/{chatId}/pin",
            cancellationToken: cancellationToken
        )).Success;
    }
}
