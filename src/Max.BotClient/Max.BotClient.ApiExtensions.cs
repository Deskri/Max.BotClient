using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Max.BotClient.Mapping;

namespace Max.BotClient
{
    internal static class BotClientApiMethodsExtensions
    {
        public static async Task<TResult> ProcessApi<TDto, TResult>(
            this IBotClient botClient,
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
            this IBotClient botClient,
            HttpMethod method,
            string path,
            object? body = null,
            CancellationToken cancellationToken = default
        ) => await botClient.SendRequest<TResponse>(method, path, body, cancellationToken);

        public static async Task ProcessApi(
            this IBotClient botClient,
            HttpMethod method,
            string path,
            object? body = null,
            CancellationToken cancellationToken = default
        ) => await botClient.SendRequest<object>(method, path, body, cancellationToken);

        public static async Task<TResult> ProcessApi<TParams, TDto, TResult>(
            this IBotClient botClient,
            HttpMethod method,
            string basePath,
            Func<TParams> createParams,
            CancellationToken cancellationToken = default
        ) where TParams : class
        {
            ApiRequestBinder.Bind(createParams(), basePath, out var path, out var body);
            var dto = await botClient.SendRequest<TDto>(method, path, body, cancellationToken);
            return dto.ToResult<TDto, TResult>();
        }

        public static async Task<TResponse> ProcessApi<TParams, TResponse>(
            this IBotClient botClient,
            HttpMethod method,
            string basePath,
            Func<TParams> createParams,
            CancellationToken cancellationToken = default
        ) where TParams : class
        {
            ApiRequestBinder.Bind(createParams(), basePath, out var path, out var body);
            return await botClient.SendRequest<TResponse>(method, path, body, cancellationToken);
        }

        public static async Task ProcessApi<TParams>(
            this IBotClient botClient,
            HttpMethod method,
            string basePath,
            Func<TParams> createParams,
            CancellationToken cancellationToken = default
        ) where TParams : class
        {
            ApiRequestBinder.Bind(createParams(), basePath, out var path, out var body);
            await botClient.SendRequest<object>(method, path, body, cancellationToken);
        }
        
        internal static async Task<TResult> PollingProcessApi<TParams, TDto, TResult>(
            this IBotClientInternal botClient,
            HttpMethod method,
            string basePath,
            Func<TParams> createParams,
            CancellationToken cancellationToken = default
        ) where TParams : class
        {
            ApiRequestBinder.Bind(createParams(), basePath, out var path, out var body);
            var dto = await botClient.PollingSendRequest<TDto>(method, path, body, cancellationToken);
            return dto.ToResult<TDto, TResult>();
        }
    }
}
