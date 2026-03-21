using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Max.BotClient.Tests.Integration.Helpers;
using Xunit;

namespace Max.BotClient.Tests.Integration.ApiMethods;

public class MembersTests
{
    private const string ApiResponseOk = """{"success":true}""";
    private const string EmptyMembers = """{"members":[],"marker":null}""";
    private const string MinimalMember = """{"user_id":1,"first_name":"Bot","is_bot":false,"last_access_time":0,"is_owner":false,"is_admin":false,"join_time":0}""";

    // ─── GetMyMembership ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetMyMembership_SendsGetToMePath()
    {
        var (client, handler) = FakeHttpHandler.Create(MinimalMember);

        await client.GetMyMembership(100L);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/chats/100/members/me");
    }

    // ─── LeaveChat ────────────────────────────────────────────────────────────

    [Fact]
    public async Task LeaveChat_SendsDeleteToMePath()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.LeaveChat(200L);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/chats/200/members/me");
    }

    // ─── GetChatMembers ───────────────────────────────────────────────────────

    [Fact]
    public async Task GetChatMembers_SendsGetToMembersPath()
    {
        var (client, handler) = FakeHttpHandler.Create(EmptyMembers);

        await client.GetChatMembers(50L);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/chats/50/members");
    }

    [Fact]
    public async Task GetChatMembers_WithCountAndMarker_InQuery()
    {
        var (client, handler) = FakeHttpHandler.Create(EmptyMembers);

        await client.GetChatMembers(50L, count: 20, marker: 999L);

        var query = handler.LastRequest!.RequestUri!.Query;
        query.Should().Contain("count=20");
        query.Should().Contain("marker=999");
    }

    [Fact]
    public async Task GetChatMembers_WithUserIds_InQuery()
    {
        var (client, handler) = FakeHttpHandler.Create(EmptyMembers);

        await client.GetChatMembers(50L, userIds: new[] { 1L, 2L, 3L });

        handler.LastRequest!.RequestUri!.Query.Should().Contain("user_ids=1,2,3");
    }

    // ─── AddChatMembers ───────────────────────────────────────────────────────

    [Fact]
    public async Task AddChatMembers_SendsPostToMembersPath()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.AddChatMembers(10L, new[] { 5L, 6L });

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/chats/10/members");
    }

    [Fact]
    public async Task AddChatMembers_UserIdsInBody()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.AddChatMembers(10L, new[] { 5L, 6L });

        var body = JsonDocument.Parse(handler.CapturedBody!);
        var ids = body.RootElement.GetProperty("user_ids");
        ids[0].GetInt64().Should().Be(5L);
        ids[1].GetInt64().Should().Be(6L);
    }

    // ─── RemoveChatMember ─────────────────────────────────────────────────────

    [Fact]
    public async Task RemoveChatMember_SendsDeleteToMembersPath()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.RemoveChatMember(30L, 7L);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/chats/30/members");
    }

    [Fact]
    public async Task RemoveChatMember_UserIdAndBlockInQuery()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.RemoveChatMember(30L, 7L, block: true);

        var query = handler.LastRequest!.RequestUri!.Query;
        query.Should().Contain("user_id=7");
        query.Should().Contain("block=true");
    }

    [Fact]
    public async Task RemoveChatMember_NullBlock_NotInQuery()
    {
        var (client, handler) = FakeHttpHandler.Create(ApiResponseOk);

        await client.RemoveChatMember(30L, 7L);

        handler.LastRequest!.RequestUri!.Query.Should().NotContain("block=");
    }
}