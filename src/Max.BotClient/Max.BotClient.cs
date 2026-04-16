using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Max.BotClient
{
    public interface IBotClient
    {
        CancellationToken GlobalCancelToken { get; }

        Task<TResponse> SendRequest<TResponse>(
            HttpMethod method,
            string path,
            object? body = null,
            CancellationToken cancellationToken = default
        );
    }

    internal interface IBotClientInternal : IBotClient
    {
        Task<TResponse> PollingSendRequest<TResponse>(
            HttpMethod method,
            string path,
            object? body = null,
            CancellationToken cancellationToken = default
        );
    }

    public partial class BotClient : IBotClientInternal, IDisposable
    {
        private readonly BotClientOptions _options;
        private readonly HttpClient _httpClient;
        private readonly bool _ownsHttpClient;
        private HttpClient? _pollingHttpClient;
        private bool _disposed;

        public string Token => _options.Token;
        public CancellationToken GlobalCancelToken { get; }

        public BotClient(
            BotClientOptions options,
            HttpClient? httpClient = null,
            CancellationToken cancellationToken = default
        )
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            GlobalCancelToken = cancellationToken;

            _ownsHttpClient = httpClient == null;
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri(_options.ApiUrl);
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _options.Token);
        }

        public BotClient(
            string token,
            HttpClient? httpClient = null,
            CancellationToken cancellationToken = default
        ) : this(new BotClientOptions(token), httpClient, cancellationToken)
        {
        }

        public async Task<TResponse> SendRequest<TResponse>(
            HttpMethod method,
            string path,
            object? body = null,
            CancellationToken cancellationToken = default
        ) => await SendRequestCore<TResponse>(_httpClient, method, path, body, cancellationToken);
        
        async Task<TResponse> IBotClientInternal.PollingSendRequest<TResponse>(
            HttpMethod method,
            string path,
            object? body,
            CancellationToken cancellationToken
        )
        {
            if (_pollingHttpClient == null)
            {
                _pollingHttpClient = new HttpClient
                {
                    BaseAddress = new Uri(_options.ApiUrl),
                    Timeout = Timeout.InfiniteTimeSpan
                };
                _pollingHttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _options.Token);
            }

            return await SendRequestCore<TResponse>(_pollingHttpClient, method, path, body, cancellationToken);
        }

        private async Task<TResponse> SendRequestCore<TResponse>(
            HttpClient httpClient,
            HttpMethod method,
            string path,
            object? body,
            CancellationToken cancellationToken
        )
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(GlobalCancelToken, cancellationToken);
            var token = cts.Token;

            MaxBotClientApiException? lastException = null;

            for (int attempt = 0; attempt <= _options.RetryCount; attempt++)
            {
                if (attempt > 0 && lastException != null)
                {
                    // Exponential backoff: 1s, 2s, 4s...
                    var delay = _options.RetryDelaySeconds * (1 << (attempt - 1));
                    await Task.Delay(TimeSpan.FromSeconds(delay), token);
                }

                using var request = new HttpRequestMessage(method, path);

                if (body != null)
                {
                    var json = JsonSerializer.Serialize(body, BotClientJsonOptions.Default);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                using var response = await httpClient.SendAsync(request, token);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<TResponse>(responseBody, BotClientJsonOptions.Default)!;
                }

                var exception = new MaxBotClientApiException(response.StatusCode, responseBody);

                if (!exception.IsRetryable)
                {
                    throw exception;
                }

                lastException = exception;
            }

            throw lastException!;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            _pollingHttpClient?.Dispose();
            if (_ownsHttpClient) _httpClient.Dispose();
        }
    }
}
