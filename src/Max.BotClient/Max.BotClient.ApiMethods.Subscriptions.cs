using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Max.BotClient.Types;

namespace Max.BotClient
{
    public static partial class BotClientApiMethods
    {
        /// <summary>
        /// Получить список подписок на вебхуки.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/subscriptions"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public static async Task<Subscription[]> GetSubscriptions(
            this BotClient botClient,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<GetSubscriptionsResponse>(
            HttpMethod.Get,
            "/subscriptions",
            cancellationToken: cancellationToken
        )).Subscriptions;

        /// <summary>
        /// Подписаться на получение обновлений через WebHook.
        /// <see href="https://dev.max.ru/docs-api/methods/POST/subscriptions"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="url">URL вебхука (должен начинаться с http(s)://).</param>
        /// <param name="updateTypes">Типы обновлений для подписки.</param>
        /// <param name="secret">Секрет для заголовка X-Max-Bot-Api-Secret (5-256 символов, A-Z, a-z, 0-9, -, _).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public static async Task<SubscribeResult> Subscribe(
            this BotClient botClient,
            string url,
            Types.UpdateType[]? updateTypes = null,
            string? secret = null,
            CancellationToken cancellationToken = default
        ) => await botClient.ProcessApi<SubscribeResult>(
            HttpMethod.Post,
            "/subscriptions",
            new SubscribeRequest { Url = url, UpdateTypes = updateTypes, Secret = secret },
            cancellationToken
        );

        /// <summary>
        /// Отписаться от получения обновлений через WebHook.
        /// <see href="https://dev.max.ru/docs-api/methods/DELETE/subscriptions"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="url">URL вебхука для удаления из подписок.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public static async Task<SubscribeResult> Unsubscribe(
            this BotClient botClient,
            string url,
            CancellationToken cancellationToken = default
        ) => await botClient.ProcessApi<UnsubscribeParams, SubscribeResult>(
            HttpMethod.Delete,
            "/subscriptions",
            () => new UnsubscribeParams { Url = url },
            cancellationToken
        );

        /// <summary>
        /// Получить обновления через long polling.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/updates"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="limit">Максимальное количество обновлений (1-1000, по умолчанию 100).</param>
        /// <param name="timeout">Тайм-аут в секундах для long polling (0-90, по умолчанию 30).</param>
        /// <param name="marker">Маркер для получения обновлений после указанной позиции.</param>
        /// <param name="types">Типы обновлений для получения.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public static async Task<(Update[], long?)> GetUpdates(
            this BotClient botClient,
            int? limit = null,
            int? timeout = null,
            long? marker = null,
            Types.UpdateType[]? types = null,
            CancellationToken cancellationToken = default
        )
        {
            var response = await botClient.ProcessApi<GetUpdatesParams, DTOs.GetUpdatesResponse, Types.GetUpdatesResponse>(
                HttpMethod.Get,
                "/updates",
                () => new GetUpdatesParams { Limit = limit, Timeout = timeout, Marker = marker, UpdateTypes = types },
                cancellationToken
            );

            return (response.Updates, response.Marker);
        }
    }
}
