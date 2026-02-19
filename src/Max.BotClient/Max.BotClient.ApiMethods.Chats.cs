using System.Collections.Generic;
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
        )
        {
            var queryParams = new List<string>();
            if (count.HasValue)
                queryParams.Add($"count={count.Value}");
            if (marker.HasValue)
                queryParams.Add($"marker={marker.Value}");

            var path = queryParams.Count > 0
                ? $"/chats?{string.Join("&", queryParams)}"
                : "/chats";

            var response = await botClient.ProcessApi<DTOs.GetChatsResponse, Types.GetChatsResponse>(
                HttpMethod.Get,
                path,
                cancellationToken: cancellationToken
            );

            return response;
        }

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
        )
        {
            var path = $"/chats/{chatId}";

            var response = await botClient.ProcessApi<DTOs.Chat, Types.Chat>(
                HttpMethod.Get,
                path,
                cancellationToken: cancellationToken
            );

            return response;
        }

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
        )
        {
            var path = $"/chats/{chatId}";

            var response = await botClient.ProcessApi<DTOs.Chat, Types.Chat>(
                new HttpMethod("PATCH"),
                path,
                request.ToDto(),
                cancellationToken
            );

            return response;
        }

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
        )
        {
            var path = $"/chats/{chatId}";

            var response = await botClient.ProcessApi<ApiResponse>(
                HttpMethod.Delete,
                path,
                cancellationToken: cancellationToken
            );

            return response.Success;
        }

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
        )
        {
            var path = $"/chats/{chatId}/actions";

            var response = await botClient.ProcessApi<ApiResponse>(
                HttpMethod.Post,
                path,
                new { action = action.ToString().ToSnakeCase() },
                cancellationToken
            );

            return response.Success;
        }

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
        )
        {
            var path = $"/chats/{chatId}/pin";

            var response = await botClient.ProcessApi<DTOs.GetPinnedMessageResponse, Types.GetPinnedMessageResponse>(
                HttpMethod.Get,
                path,
                cancellationToken: cancellationToken
            );

            return response?.Message;
        }

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
        )
        {
            var path = $"/chats/{chatId}/pin";

            var requestBody = notify.HasValue
                ? new { message_id = messageId, notify = notify.Value }
                : (object)new { message_id = messageId };

            var response = await botClient.ProcessApi<ApiResponse>(
                new HttpMethod("PUT"),
                path,
                requestBody,
                cancellationToken
            );

            return response.Success;
        }

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
        )
        {
            var path = $"/chats/{chatId}/pin";

            var response = await botClient.ProcessApi<ApiResponse>(
                HttpMethod.Delete,
                path,
                cancellationToken: cancellationToken
            );

            return response.Success;
        }
    }
}
