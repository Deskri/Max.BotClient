using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Max.BotClient.Tests.Integration.Helpers;
using Max.BotClient.Types;
using Xunit;

namespace Max.BotClient.Tests.Integration.ApiMethods;

public class ChatsTests
{
    private const string ApiResponseOk = """{"success":true}""";
    private const string EmptyChats = """{"chats":[],"marker":null}""";
    private const string MinimalChat = """{"chat_id":0,"type":"chat","status":"active","last_event_time":0,"participants_count":0,"is_public":false}""";

    // ─── GetChats ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetChats_SendsGetToChatsPath()
    {
        var (client, handler) = FakeHttpHandler.Create(EmptyChats);

        await client.GetChats();

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/chats");
    }

    [Fact]
    public async Task GetChats_WithParams_IncludesCountAndMarker()
    {
        var (client, handler) = FakeHttpHandler.Create(EmptyChats);

        await client.GetChats(count: 10, marker: 500L);

        var query = handler.LastRequest!.RequestUri!.Query;
        query.Should().Contain("count=10");
        query.Should().Contain("marker=500");
    }

    [Fact]
    public async Task GetChats_NullParams_NoQueryString()
    {
        var (client, handler) = FakeHttpHandler.Create(EmptyChats);

        await client.GetChats();

        handler.LastRequest!.RequestUri!.Query.Should().BeEmpty();
    }

    // ─── GetChat ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetChat_SendsGetToChatPath()
    {
        var (client, handler) = FakeHttpHandler.Create(MinimalChat);

        await client.GetChat(99L);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/chats/99");
    }

    // ─── DeleteChat ───────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteChat_SendsDeleteToChatPath()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.DeleteChat(77L);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/chats/77");
    }

    // ─── SendAction ───────────────────────────────────────────────────────────

    [Fact]
    public async Task SendAction_SendsPostToActionsPath()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.SendAction(55L, SenderAction.TypingOn);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/chats/55/actions");
    }

    [Fact]
    public async Task SendAction_ActionSnakeCaseInBody()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.SendAction(1L, SenderAction.TypingOn);

        var body = JsonDocument.Parse(handler.CapturedBody!);
        body.RootElement.GetProperty("action").GetString().Should().Be("typing_on");
    }

    // ─── PinMessage ───────────────────────────────────────────────────────────

    [Fact]
    public async Task PinMessage_SendsPutToPinPath()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.PinMessage(10L, "msg1", notify: true);

        handler.LastRequest!.Method.Should().Be(new HttpMethod("PUT"));
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/chats/10/pin");
    }

    [Fact]
    public async Task PinMessage_MessageIdAndNotifyInBody()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.PinMessage(1L, "msgABC", notify: true);

        var body = JsonDocument.Parse(handler.CapturedBody!);
        body.RootElement.GetProperty("message_id").GetString().Should().Be("msgABC");
        body.RootElement.GetProperty("notify").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public async Task PinMessage_NullNotify_NotInBody()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.PinMessage(1L, "msg1");

        var body = JsonDocument.Parse(handler.CapturedBody!);
        body.RootElement.TryGetProperty("notify", out _).Should().BeFalse();
    }

    // ─── UnpinMessage ─────────────────────────────────────────────────────────

    [Fact]
    public async Task UnpinMessage_SendsDeleteToPinPath()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.UnpinMessage(33L);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/chats/33/pin");
    }

    // ─── GetPinnedMessage ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetPinnedMessage_SendsGetToPinPath()
    {
        var (client, handler) = FakeHttpHandler.Create("""{"message":null}""");

        await client.GetPinnedMessage(22L);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/chats/22/pin");
    }
}