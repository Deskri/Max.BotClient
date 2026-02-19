using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Max.BotClient.Mapping;

namespace Max.BotClient
{
    internal static class BotClientApiMethodsExtensions
    {
        public static async Task<TResult> ProcessApi<TDto, TResult>(
            this BotClient botClient,
            HttpMethod method,
            string path,
            object? body = null,
            CancellationToken cancellationToken = default
        )
        {
            var dto = await botClient.SendRequest<TDto>(method, path, body, cancellationToken);
            return dto.ToResult<TDto, TResult>();
        }

        public static async Task<TResponse> ProcessApi<TResponse>(
            this BotClient botClient,
            HttpMethod method,
            string path,
            object? body = null,
            CancellationToken cancellationToken = default
        ) => await botClient.SendRequest<TResponse>(method, path, body, cancellationToken);

        public static async Task ProcessApi(
            this BotClient botClient,
            HttpMethod method,
            string path,
            object? body = null,
            CancellationToken cancellationToken = default
        ) => await botClient.SendRequest<object>(method, path, body, cancellationToken);
    }
}