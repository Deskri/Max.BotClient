using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Max.BotClient.DTOs;

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
            options.Converters.Add(new UpdateConverter());
            options.Converters.Add(new AttachmentConverter());
            options.Converters.Add(new ButtonConverter());
            options.Converters.Add(new MarkupElementConverter());
            options.Converters.Add(new AttachmentRequestConverter());

            return options;
        }
    }

    internal class UpdateConverter : JsonConverter<IUpdate>
    {
        public override IUpdate? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty("update_type", out var typeElement))
                return null;

            return typeElement.GetString() switch
            {
                "message_created"    => root.Deserialize<MessageCreatedUpdate>(options),
                "message_callback"   => root.Deserialize<MessageCallbackUpdate>(options),
                "message_edited"     => root.Deserialize<MessageEditedUpdate>(options),
                "message_removed"    => root.Deserialize<MessageRemovedUpdate>(options),
                "bot_added"          => root.Deserialize<BotAddedUpdate>(options),
                "bot_removed"        => root.Deserialize<BotRemovedUpdate>(options),
                "dialog_muted"       => root.Deserialize<DialogMutedUpdate>(options),
                "dialog_unmuted"     => root.Deserialize<DialogUnmutedUpdate>(options),
                "dialog_cleared"     => root.Deserialize<DialogClearedUpdate>(options),
                "dialog_removed"     => root.Deserialize<DialogRemovedUpdate>(options),
                "user_added"         => root.Deserialize<UserAddedUpdate>(options),
                "user_removed"       => root.Deserialize<UserRemovedUpdate>(options),
                "bot_started"        => root.Deserialize<BotStartedUpdate>(options),
                "bot_stopped"        => root.Deserialize<BotStoppedUpdate>(options),
                "chat_title_changed" => root.Deserialize<ChatTitleChangedUpdate>(options),
                _                    => null
            };
        }

        public override void Write(Utf8JsonWriter writer, IUpdate value, JsonSerializerOptions options)
            => throw new NotSupportedException("Serialization of IUpdate is not supported.");
    }

    internal class AttachmentConverter : JsonConverter<IAttachment>
    {
        public override IAttachment? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty("type", out var typeElement))
                return null;

            return typeElement.GetString() switch
            {
                "image"           => root.Deserialize<PhotoAttachment>(options),
                "video"           => root.Deserialize<VideoAttachment>(options),
                "audio"           => root.Deserialize<AudioAttachment>(options),
                "file"            => root.Deserialize<FileAttachment>(options),
                "sticker"         => root.Deserialize<StickerAttachment>(options),
                "contact"         => root.Deserialize<ContactAttachment>(options),
                "share"           => root.Deserialize<ShareAttachment>(options),
                "location"        => root.Deserialize<LocationAttachment>(options),
                "inline_keyboard" => root.Deserialize<InlineKeyboardAttachment>(options),
                _                 => null
            };
        }

        public override void Write(Utf8JsonWriter writer, IAttachment value, JsonSerializerOptions options)
            => JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }

    internal class ButtonConverter : JsonConverter<IButton>
    {
        public override IButton? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty("type", out var typeElement))
                return null;

            return typeElement.GetString() switch
            {
                "callback"             => root.Deserialize<CallbackButton>(options),
                "link"                 => root.Deserialize<LinkButton>(options),
                "request_geo_location" => root.Deserialize<RequestGeoLocationButton>(options),
                "request_contact"      => root.Deserialize<RequestContactButton>(options),
                "open_app"             => root.Deserialize<OpenAppButton>(options),
                "message"              => root.Deserialize<MessageButton>(options),
                _                      => null
            };
        }

        public override void Write(Utf8JsonWriter writer, IButton value, JsonSerializerOptions options)
            => JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }

    internal class MarkupElementConverter : JsonConverter<IMarkupElement>
    {
        public override IMarkupElement? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty("type", out var typeElement))
                return null;

            return typeElement.GetString() switch
            {
                "strong"        => root.Deserialize<StrongMarkupElement>(options),
                "emphasized"    => root.Deserialize<EmphasizedMarkupElement>(options),
                "monospaced"    => root.Deserialize<MonospacedMarkupElement>(options),
                "link"          => root.Deserialize<LinkMarkupElement>(options),
                "strikethrough" => root.Deserialize<StrikethroughMarkupElement>(options),
                "underline"     => root.Deserialize<UnderlineMarkupElement>(options),
                "user_mention"  => root.Deserialize<UserMentionMarkupElement>(options),
                _               => null
            };
        }

        public override void Write(Utf8JsonWriter writer, IMarkupElement value, JsonSerializerOptions options)
            => JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }

    internal class AttachmentRequestConverter : JsonConverter<IAttachmentRequest>
    {
        public override IAttachmentRequest? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => throw new NotSupportedException("Deserialization of IAttachmentRequest is not supported.");

        public override void Write(Utf8JsonWriter writer, IAttachmentRequest value, JsonSerializerOptions options)
            => JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}