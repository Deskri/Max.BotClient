using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Max.BotClient
{
    public static partial class BotClientApiMethods
    {
        /// <summary>
        /// Получить информацию о текущем боте.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/me"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public static async Task<Types.BotInfo> GetMe(
            this IBotClient botClient,
            CancellationToken cancellationToken = default
        ) => await botClient.ProcessApi<DTOs.UserBotInfo, Types.BotInfo>(
            HttpMethod.Get,
            "/me",
            cancellationToken: cancellationToken
        );

        /// <summary>
        /// Получить URL для загрузки файла.
        /// <see href="https://dev.max.ru/docs-api/methods/POST/uploads"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="type">Тип загружаемого файла.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>URL для загрузки и токен (для video/audio).</returns>
        public static async Task<Types.UploadResult> GetUploadUrl(
            this IBotClient botClient,
            Types.UploadType type,
            CancellationToken cancellationToken = default
        ) => await botClient.ProcessApi<GetUploadUrlParams, Types.UploadResult>(
            HttpMethod.Post,
            "/uploads",
            () => new GetUploadUrlParams { Type = type },
            cancellationToken
        );

        /// <summary>
        /// Получить подробную информацию о прикреплённом видео.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/videos/-videoToken-"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="videoToken">Токен видео-вложения.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Подробная информация о видео, включая URL-адреса воспроизведения и метаданные.</returns>
        public static async Task<Types.VideoInfo> GetVideo(
            this IBotClient botClient,
            string videoToken,
            CancellationToken cancellationToken = default
        ) => await botClient.ProcessApi<DTOs.VideoInfo, Types.VideoInfo>(
            HttpMethod.Get,
            $"/videos/{Uri.EscapeDataString(videoToken)}",
            cancellationToken: cancellationToken
        );
    }
}
