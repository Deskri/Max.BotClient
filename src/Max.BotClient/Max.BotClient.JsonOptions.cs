using System.Text.Json;
using System.Text.Json.Serialization;

namespace Max.BotClient
{
    /// <summary>
    /// Общие настройки JSON сериализации для Max API.
    /// </summary>
    public static class BotClientJsonOptions
    {
        /// <summary>
        /// Настройки по умолчанию для десериализации ответов Max API.
        /// Использует snake_case для имён свойств.
        /// </summary>
        public static JsonSerializerOptions Default { get; } = CreateDefault();

        private static JsonSerializerOptions CreateDefault()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));

            return options;
        }
    }
}
