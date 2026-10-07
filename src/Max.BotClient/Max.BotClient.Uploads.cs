using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Max.BotClient
{
    /// <summary>
    /// Загрузка медиафайлов через POST /uploads.
    /// <see href="https://dev.max.ru/docs-api/methods/POST/uploads"/>
    /// </summary>
    public partial class BotClient
    {
        private const long MaxImageSizeBytes = 50L * 1024 * 1024;

        /// <summary>
        /// Загружает изображение из локального файла и возвращает токен для отправки в сообщении.
        /// </summary>
        /// <param name="filePath">Путь к локальному файлу.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Токен загруженного изображения.</returns>
        public async Task<string> UploadImageAsync(
            string filePath,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Image file not found.", filePath);

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length > MaxImageSizeBytes)
                throw new InvalidOperationException("Image exceeds MAX 50 MB limit.");

            using (var stream = File.OpenRead(filePath))
            {
                return await UploadImageAsync(stream, fileInfo.Name, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Загружает изображение из потока и возвращает токен для отправки в сообщении.
        /// </summary>
        /// <param name="content">Поток с содержимым файла.</param>
        /// <param name="fileName">Имя файла (используется для определения content-type).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Токен загруженного изображения.</returns>
        public async Task<string> UploadImageAsync(
            Stream content,
            string fileName,
            CancellationToken cancellationToken = default
        )
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = "photo.jpg";

            var upload = await BotClientApiMethods.GetUploadUrl(this, Types.UploadType.Image, cancellationToken).ConfigureAwait(false);
            if (upload == null || string.IsNullOrWhiteSpace(upload.Url))
                throw new InvalidOperationException("Empty upload url received from POST /uploads.");

            using (var form = new MultipartFormDataContent())
            {
                var fileContent = new StreamContent(content);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(GetImageMediaType(fileName));
                form.Add(fileContent, "data", fileName);

                using (var request = new HttpRequestMessage(HttpMethod.Post, upload.Url))
                {
                    request.Content = form;
                    // Authorization уже выставлен в DefaultRequestHeaders _httpClient,
                    // дублировать заголовок нельзя.
                    using (var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
                    {
                        var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        if (!response.IsSuccessStatusCode)
                            throw new MaxBotClientApiException(response.StatusCode, body);

                        var token = ParseImageUploadToken(body);
                        if (string.IsNullOrWhiteSpace(token))
                            throw new InvalidOperationException("Unexpected image upload response: " + body);

                        return token;
                    }
                }
            }
        }

        private static string GetImageMediaType(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(ext))
                return "image/jpeg";

            switch (ext.ToLowerInvariant())
            {
                case ".png": return "image/png";
                case ".gif": return "image/gif";
                case ".tif":
                case ".tiff": return "image/tiff";
                case ".bmp": return "image/bmp";
                case ".heic": return "image/heic";
                case ".jpg":
                case ".jpeg":
                default: return "image/jpeg";
            }
        }

        /// <summary>
        /// Разбирает ответ загрузки изображения.
        /// Успешный ответ: {"photos": {"&lt;photoId&gt;": {"token": "..."}}}.
        /// </summary>
        private static string ParseImageUploadToken(string body)
        {
            using (var doc = JsonDocument.Parse(body))
            {
                var root = doc.RootElement;

                // Прямой токен: {"token": "..."}
                if (root.ValueKind == JsonValueKind.Object
                    && root.TryGetProperty("token", out var directToken)
                    && directToken.ValueKind == JsonValueKind.String)
                {
                    return directToken.GetString();
                }

                // Стандартный ответ: {"photos": {...}}
                if (root.ValueKind == JsonValueKind.Object
                    && root.TryGetProperty("photos", out var photos)
                    && photos.ValueKind == JsonValueKind.Object)
                {
                    foreach (var photo in photos.EnumerateObject())
                    {
                        if (photo.Value.ValueKind == JsonValueKind.Object
                            && photo.Value.TryGetProperty("token", out var token)
                            && token.ValueKind == JsonValueKind.String)
                        {
                            var value = token.GetString();
                            if (!string.IsNullOrWhiteSpace(value))
                                return value;
                        }

                        if (photo.Value.ValueKind == JsonValueKind.String)
                        {
                            var value = photo.Value.GetString();
                            if (!string.IsNullOrWhiteSpace(value))
                                return value;
                        }
                    }
                }

                return null;
            }
        }
    }
}