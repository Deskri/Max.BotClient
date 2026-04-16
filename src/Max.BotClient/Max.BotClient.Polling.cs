using System;
using System.Threading;
using System.Threading.Tasks;
using Max.BotClient.Types;

namespace Max.BotClient
{
    /// <summary>
    /// Опции настройки polling.
    /// </summary>
    public class ReceiverOptions
    {
        /// <summary>
        /// Максимальное количество обновлений за один запрос (1-1000, по умолчанию 100).
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Тайм-аут в секундах для long polling (0-90, по умолчанию 30).
        /// </summary>
        public int Timeout { get; set; } = 30;

        /// <summary>
        /// Фильтр типов обновлений. Если null — получать все типы.
        /// </summary>
        public UpdateType[]? AllowedUpdates { get; set; }

        /// <summary>
        /// Пропустить накопившиеся обновления при старте.
        /// </summary>
        public bool DropPendingUpdates { get; set; }
    }

    public static partial class BotClientApiMethods
    {
        /// <summary>
        /// Запускает получение обновлений через long polling в фоновом режиме (не блокирует).
        /// <see href="https://dev.max.ru/docs-api/methods/GET/updates"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="updateHandler">Обработчик обновлений.</param>
        /// <param name="errorHandler">Обработчик ошибок (необязательно).</param>
        /// <param name="options">Опции polling.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public static void StartReceiving(
            this IBotClient botClient,
            Func<IBotClient, Update, CancellationToken, Task> updateHandler,
            Func<IBotClient, Exception, CancellationToken, Task>? errorHandler = null,
            ReceiverOptions? options = null,
            CancellationToken cancellationToken = default
        ) => Task.Run(() => 
                botClient.ReceiveAsync(
                    updateHandler, 
                    errorHandler, 
                    options, 
                    cancellationToken
                ), 
            cancellationToken
        );

        /// <summary>
        /// Получает обновления через long polling (блокирует до отмены).
        /// <see href="https://dev.max.ru/docs-api/methods/GET/updates"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="updateHandler">Обработчик обновлений.</param>
        /// <param name="errorHandler">Обработчик ошибок (необязательно).</param>
        /// <param name="options">Опции polling.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public static async Task ReceiveAsync(
            this IBotClient botClient,
            Func<IBotClient, Update, CancellationToken, Task> updateHandler,
            Func<IBotClient, Exception, CancellationToken, Task>? errorHandler = null,
            ReceiverOptions? options = null,
            CancellationToken cancellationToken = default
        )
        {
            options = options ?? new ReceiverOptions();
            long? marker = null;

            // Сброс накопившихся обновлений
            if (options.DropPendingUpdates)
            {
                try
                {
                    var (_, newMarker) = await botClient.GetUpdates(
                        limit: 1,
                        timeout: 0,
                        marker: null,
                        types: options.AllowedUpdates,
                        cancellationToken: cancellationToken
                    );
                    marker = newMarker;
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (errorHandler != null)
                            await errorHandler(botClient, ex, cancellationToken);
                    }
                    catch
                    {
                        // Не позволяем errorHandler убить polling loop
                    }
                }
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                Update[] updates;

                try
                {
                    var result = await botClient.GetUpdates(
                        limit: options.Limit,
                        timeout: options.Timeout,
                        marker: marker,
                        types: options.AllowedUpdates,
                        cancellationToken: cancellationToken
                    );

                    updates = result.Item1;
                    marker = result.Item2 ?? marker;
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (errorHandler != null)
                            await errorHandler(botClient, ex, cancellationToken);
                    }
                    catch
                    {
                        // Не позволяем errorHandler убить polling loop
                    }

                    // Backoff перед повторной попыткой, чтобы не забивать API при длительном сбое
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }

                    continue;
                }

                foreach (var update in updates)
                {
                    try
                    {
                        await updateHandler(botClient, update, cancellationToken);
                    }
                    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            if (errorHandler != null)
                                await errorHandler(botClient, ex, cancellationToken);
                        }
                        catch
                        {
                            // Не позволяем errorHandler убить polling loop
                        }
                    }
                }
            }
        }
    }
}
