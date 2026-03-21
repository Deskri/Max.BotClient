using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Max.BotClient.Tests.Integration.Helpers;
using Max.BotClient.Types;
using Xunit;

namespace Max.BotClient.Tests.Integration.ApiMethods;

public class MessagesTests
{
    private const string ApiResponseOk = """{"success":true}""";
    private const string EmptyMessages = """{"messages":[]}""";
    private const string MinimalMessage = """{"recipient":{"chat_type":"chat"},"body":{"mid":"m1","seq":1}}""";

    // ─── GetMessages (by chat) ────────────────────────────────────────────────

    [Fact]
    public async Task GetMessages_ByChat_SendsGetWithChatIdParam()
    {
        var (client, handler) = FakeHttpHandler.Create(EmptyMessages);

        await client.GetMessages(chatId: 5L, from: 100L, to: 200L, count: 10);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/messages");
        var query = handler.LastRequest.RequestUri.Query;
        query.Should().Contain("chat_id=5");
        query.Should().Contain("from=100");
        query.Should().Contain("to=200");
        query.Should().Contain("count=10");
    }

    [Fact]
    public async Task GetMessages_NullOptionals_OnlyChatIdInQuery()
    {
        var (client, handler) = FakeHttpHandler.Create(EmptyMessages);

        await client.GetMessages(chatId: 7L);

        var query = handler.LastRequest!.RequestUri!.Query;
        query.Should().Contain("chat_id=7");
        query.Should().NotContain("from=");
        query.Should().NotContain("to=");
        query.Should().NotContain("count=");
    }

    // ─── GetMessages (by IDs) ─────────────────────────────────────────────────

    [Fact]
    public async Task GetMessages_ByIds_SendsGetWithMessageIdsParam()
    {
        var (client, handler) = FakeHttpHandler.Create(EmptyMessages);

        await client.GetMessages(new[] { "mid1", "mid2" });

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/messages");
        handler.LastRequest.RequestUri.Query.Should().Contain("message_ids=mid1,mid2");
    }

    // ─── GetMessage ───────────────────────────────────────────────────────────

    [Fact]
    public async Task GetMessage_SendsGetToMessagePath()
    {
        var (client, handler) = FakeHttpHandler.Create(MinimalMessage);

        await client.GetMessage("abc123");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/messages/abc123");
    }

    [Fact]
    public async Task GetMessage_SpecialCharsInId_UriEncoded()
    {
        var (client, handler) = FakeHttpHandler.Create(MinimalMessage);

        await client.GetMessage("mid/with/slashes");

        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/messages/mid%2Fwith%2Fslashes");
    }

    // ─── SendMessage ──────────────────────────────────────────────────────────

    [Fact]
    public async Task SendMessage_ToChat_UsesChatIdQueryParam()
    {
        var (client, handler) = FakeHttpHandler.Create("""{"message":null}""");
        var message = new Message("Hello").ToChat();

        await client.SendMessage(42L, message);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/messages");
        var query = handler.LastRequest.RequestUri.Query;
        query.Should().Contain("chat_id=42");
        query.Should().NotContain("user_id=");
    }

    [Fact]
    public async Task SendMessage_ToUser_UsesUserIdQueryParam()
    {
        var (client, handler) = FakeHttpHandler.Create("""{"message":null}""");
        var message = new Message("Hi");  // default RecipientType.User

        await client.SendMessage(99L, message);

        var query = handler.LastRequest!.RequestUri!.Query;
        query.Should().Contain("user_id=99");
        query.Should().NotContain("chat_id=");
    }

    [Fact]
    public async Task SendMessage_HasTextInBody()
    {
        var (client, handler) = FakeHttpHandler.Create("""{"message":null}""");
        var message = new Message("Test text").ToChat();

        await client.SendMessage(1L, message);

        var body = JsonDocument.Parse(handler.CapturedBody!);
        body.RootElement.GetProperty("text").GetString().Should().Be("Test text");
    }

    [Fact]
    public async Task SendMessage_DisableLinkPreview_InQuery()
    {
        var (client, handler) = FakeHttpHandler.Create("""{"message":null}""");
        var message = new Message("msg").ToChat();

        await client.SendMessage(1L, message, disableLinkPreview: true);

        handler.LastRequest!.RequestUri!.Query.Should().Contain("disable_link_preview=true");
    }

    // ─── EditMessage ──────────────────────────────────────────────────────────

    [Fact]
    public async Task EditMessage_SendsPutWithMessageIdQuery()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);
        var message = new Message("Updated");

        await client.EditMessage("msgXYZ", message);

        handler.LastRequest!.Method.Should().Be(new HttpMethod("PUT"));
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/messages");
        handler.LastRequest.RequestUri.Query.Should().Contain("message_id=msgXYZ");
    }

    // ─── DeleteMessage ────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteMessage_SendsDeleteWithMessageIdQuery()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.DeleteMessage("delMsg1");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/messages");
        handler.LastRequest.RequestUri.Query.Should().Contain("message_id=delMsg1");
    }

    // ─── AnswerCallback ───────────────────────────────────────────────────────

    [Fact]
    public async Task AnswerCallback_SendsPostWithCallbackIdQuery()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.AnswerCallback("cb42", notification: "Done!");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/answers");
        handler.LastRequest.RequestUri.Query.Should().Contain("callback_id=cb42");
    }

    [Fact]
    public async Task AnswerCallback_NotificationInBody()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.AnswerCallback("cb1", notification: "ok");

        var body = JsonDocument.Parse(handler.CapturedBody!);
        body.RootElement.GetProperty("notification").GetString().Should().Be("ok");
    }
}