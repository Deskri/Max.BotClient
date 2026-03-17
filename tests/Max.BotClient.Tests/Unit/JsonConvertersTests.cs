using System;
using System.Text.Json;
using FluentAssertions;
using Max.BotClient.DTOs;
using Xunit;

namespace Max.BotClient.Tests.Unit;

public class JsonConvertersTests
{
    private static readonly JsonSerializerOptions Options = BotClientJsonOptions.Default;

    // ─── UpdateConverter ─────────────────────────────────────────────────────────

    [Theory]
    [InlineData("message_created",    typeof(MessageCreatedUpdate))]
    [InlineData("message_callback",   typeof(MessageCallbackUpdate))]
    [InlineData("message_edited",     typeof(MessageEditedUpdate))]
    [InlineData("message_removed",    typeof(MessageRemovedUpdate))]
    [InlineData("bot_added",          typeof(BotAddedUpdate))]
    [InlineData("bot_removed",        typeof(BotRemovedUpdate))]
    [InlineData("dialog_muted",       typeof(DialogMutedUpdate))]
    [InlineData("dialog_unmuted",     typeof(DialogUnmutedUpdate))]
    [InlineData("dialog_cleared",     typeof(DialogClearedUpdate))]
    [InlineData("dialog_removed",     typeof(DialogRemovedUpdate))]
    [InlineData("user_added",         typeof(UserAddedUpdate))]
    [InlineData("user_removed",       typeof(UserRemovedUpdate))]
    [InlineData("bot_started",        typeof(BotStartedUpdate))]
    [InlineData("bot_stopped",        typeof(BotStoppedUpdate))]
    [InlineData("chat_title_changed", typeof(ChatTitleChangedUpdate))]
    public void UpdateConverter_DeserializesCorrectType(string updateType, Type expectedType)
    {
        var json = $$"""{"update_type":"{{updateType}}","timestamp":1000}""";

        var result = JsonSerializer.Deserialize<IUpdate>(json, Options);

        result.Should().BeOfType(expectedType);
        result!.Timestamp.Should().Be(1000);
    }

    [Fact]
    public void UpdateConverter_MessageCreated_MapsFields()
    {
        var json = """
            {
              "update_type": "message_created",
              "timestamp": 9999,
              "message": {
                "recipient": { "chat_type": "chat", "chat_id": 42 },
                "timestamp": 9999,
                "body": { "mid": "abc123", "seq": 5, "text": "Hello" }
              }
            }
            """;

        var result = JsonSerializer.Deserialize<IUpdate>(json, Options) as MessageCreatedUpdate;

        result.Should().NotBeNull();
        result!.Timestamp.Should().Be(9999);
        result.Message.Body!.Text.Should().Be("Hello");
        result.Message.Body.Mid.Should().Be("abc123");
    }

    [Fact]
    public void UpdateConverter_UnknownType_ReturnsNull()
    {
        var json = """{"update_type":"unknown_future_event","timestamp":1}""";

        var result = JsonSerializer.Deserialize<IUpdate>(json, Options);

        result.Should().BeNull();
    }

    [Fact]
    public void UpdateConverter_Write_ThrowsNotSupported()
    {
        IUpdate update = new MessageCreatedUpdate { Timestamp = 1 };

        var act = () => JsonSerializer.Serialize(update, Options);

        act.Should().Throw<NotSupportedException>();
    }

    // ─── AttachmentConverter ─────────────────────────────────────────────────────

    [Theory]
    [InlineData("image",           typeof(PhotoAttachment))]
    [InlineData("video",           typeof(VideoAttachment))]
    [InlineData("audio",           typeof(AudioAttachment))]
    [InlineData("file",            typeof(FileAttachment))]
    [InlineData("sticker",         typeof(StickerAttachment))]
    [InlineData("contact",         typeof(ContactAttachment))]
    [InlineData("share",           typeof(ShareAttachment))]
    [InlineData("location",        typeof(LocationAttachment))]
    [InlineData("inline_keyboard", typeof(InlineKeyboardAttachment))]
    public void AttachmentConverter_DeserializesCorrectType(string type, Type expectedType)
    {
        var json = $$"""{"type":"{{type}}"}""";

        var result = JsonSerializer.Deserialize<IAttachment>(json, Options);

        result.Should().BeOfType(expectedType);
    }

    [Fact]
    public void AttachmentConverter_Photo_MapsPayload()
    {
        var json = """
            {
              "type": "image",
              "payload": { "photo_id": 7, "token": "tok", "url": "https://example.com/photo.jpg" }
            }
            """;

        var result = JsonSerializer.Deserialize<IAttachment>(json, Options) as PhotoAttachment;

        result.Should().NotBeNull();
        result!.Payload.PhotoId.Should().Be(7);
        result.Payload.Token.Should().Be("tok");
    }

    [Fact]
    public void AttachmentConverter_UnknownType_ReturnsNull()
    {
        var result = JsonSerializer.Deserialize<IAttachment>("""{"type":"hologram"}""", Options);

        result.Should().BeNull();
    }

    // ─── ButtonConverter ─────────────────────────────────────────────────────────

    [Theory]
    [InlineData("callback",             typeof(CallbackButton))]
    [InlineData("link",                 typeof(LinkButton))]
    [InlineData("request_geo_location", typeof(RequestGeoLocationButton))]
    [InlineData("request_contact",      typeof(RequestContactButton))]
    [InlineData("open_app",             typeof(OpenAppButton))]
    [InlineData("message",              typeof(MessageButton))]
    public void ButtonConverter_DeserializesCorrectType(string type, Type expectedType)
    {
        var json = $$"""{"type":"{{type}}","text":"Click me"}""";

        var result = JsonSerializer.Deserialize<IButton>(json, Options);

        result.Should().BeOfType(expectedType);
        result!.Text.Should().Be("Click me");
    }

    [Fact]
    public void ButtonConverter_Callback_MapsPayload()
    {
        var json = """{"type":"callback","text":"OK","payload":"action_ok"}""";

        var result = JsonSerializer.Deserialize<IButton>(json, Options) as CallbackButton;

        result.Should().NotBeNull();
        result!.Payload.Should().Be("action_ok");
    }

    [Fact]
    public void ButtonConverter_Write_IncludesTypeAndFields()
    {
        IButton button = new CallbackButton { Text = "Yes", Payload = "yes" };

        var json = JsonSerializer.Serialize(button, Options);

        json.Should().Contain("\"type\":\"callback\"");
        json.Should().Contain("\"text\":\"Yes\"");
        json.Should().Contain("\"payload\":\"yes\"");
    }

    [Fact]
    public void ButtonConverter_Write_LinkButton()
    {
        IButton button = new LinkButton { Text = "Go", Url = "https://example.com" };

        var json = JsonSerializer.Serialize(button, Options);

        json.Should().Contain("\"type\":\"link\"");
        json.Should().Contain("\"url\":\"https://example.com\"");
    }

    // ─── MarkupElementConverter ───────────────────────────────────────────────────

    [Theory]
    [InlineData("strong",        typeof(StrongMarkupElement))]
    [InlineData("emphasized",    typeof(EmphasizedMarkupElement))]
    [InlineData("monospaced",    typeof(MonospacedMarkupElement))]
    [InlineData("link",          typeof(LinkMarkupElement))]
    [InlineData("strikethrough", typeof(StrikethroughMarkupElement))]
    [InlineData("underline",     typeof(UnderlineMarkupElement))]
    [InlineData("user_mention",  typeof(UserMentionMarkupElement))]
    public void MarkupElementConverter_DeserializesCorrectType(string type, Type expectedType)
    {
        var json = $$"""{"type":"{{type}}","from":2,"length":5}""";

        var result = JsonSerializer.Deserialize<IMarkupElement>(json, Options);

        result.Should().BeOfType(expectedType);
        result!.From.Should().Be(2);
        result.Length.Should().Be(5);
    }

    [Fact]
    public void MarkupElementConverter_Link_MapsUrl()
    {
        var json = """{"type":"link","from":0,"length":4,"url":"https://example.com"}""";

        var result = JsonSerializer.Deserialize<IMarkupElement>(json, Options) as LinkMarkupElement;

        result.Should().NotBeNull();
        result!.Url.Should().Be("https://example.com");
    }

    // ─── AttachmentRequestConverter ───────────────────────────────────────────────

    [Fact]
    public void AttachmentRequestConverter_Write_Photo()
    {
        IAttachmentRequest request = new PhotoAttachmentRequest
        {
            Payload = new PhotoAttachmentRequestPayload { Url = "https://example.com/img.jpg" }
        };

        var json = JsonSerializer.Serialize(request, Options);

        json.Should().Contain("\"type\":\"image\"");
        json.Should().Contain("\"url\":\"https://example.com/img.jpg\"");
    }

    [Fact]
    public void AttachmentRequestConverter_Write_InlineKeyboard()
    {
        IAttachmentRequest request = new InlineKeyboardAttachmentRequest
        {
            Payload = new InlineKeyboardAttachmentRequestPayload
            {
                Buttons = [[new CallbackButton { Text = "Hi", Payload = "hi" }]]
            }
        };

        var json = JsonSerializer.Serialize(request, Options);

        json.Should().Contain("\"type\":\"inline_keyboard\"");
        json.Should().Contain("\"text\":\"Hi\"");
    }

    [Fact]
    public void AttachmentRequestConverter_Read_ThrowsNotSupported()
    {
        var act = () => JsonSerializer.Deserialize<IAttachmentRequest>("""{"type":"image"}""", Options);

        act.Should().Throw<NotSupportedException>();
    }
}