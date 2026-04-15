#if NET10_0_OR_GREATER
using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Max.BotClient
{
    public static class BotClientServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует <see cref="BotClient"/> и <see cref="IBotClient"/> в DI-контейнере.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="token">Токен бота.</param>
        /// <param name="configure">Дополнительная настройка параметров (RetryCount, RetryDelaySeconds и др.).</param>
        public static IServiceCollection AddMaxBotClient(
            this IServiceCollection services,
            string token,
            Action<BotClientOptions>? configure = null)
        {
            ArgumentNullException.ThrowIfNull(services);

            var options = new BotClientOptions(token);
            configure?.Invoke(options);

            services.TryAddSingleton(options);

            services.AddHttpClient("MaxBotClient");

            services.TryAddSingleton<IBotClient>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = factory.CreateClient("MaxBotClient");
                return new BotClient(options, httpClient);
            });

            services.TryAddSingleton<BotClient>(sp => (BotClient)sp.GetRequiredService<IBotClient>());

            return services;
        }
    }
}
#endif