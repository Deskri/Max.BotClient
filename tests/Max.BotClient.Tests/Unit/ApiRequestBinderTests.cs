using System;
using System.Collections.Generic;
using FluentAssertions;
using Max.BotClient.DTOs;
using Max.BotClient.Types;
using Xunit;

namespace Max.BotClient.Tests.Unit;

public class ApiRequestBinderTests
{
    // ─── QueryParam: null → пропускается ────────────────────────────────────

    [Fact]
    public void AllNullQueryParams_PathUnchanged()
    {
        ApiRequestBinder.Bind(new GetChatsParams(), "/chats", out var path, out var body);

        path.Should().Be("/chats");
        body.Should().BeNull();
    }

    // ─── QueryParam: числа ───────────────────────────────────────────────────

    [Fact]
    public void SingleIntQueryParam_AppendedToPath()
    {
        ApiRequestBinder.Bind(new GetChatsParams { Count = 10 }, "/chats", out var path, out _);

        path.Should().Be("/chats?count=10");
    }

    [Fact]
    public void MultipleLongQueryParams_JoinedWithAmpersand()
    {
        ApiRequestBinder.Bind(
            new GetChatsParams { Count = 10, Marker = 999L },
            "/chats", out var path, out _);

        path.Should().Be("/chats?count=10&marker=999");
    }

    [Fact]
    public void NullableWithValue_IncludedInQuery()
    {
        ApiRequestBinder.Bind(
            new GetMessagesByChatParams { ChatId = 5L, From = 1000L, To = 2000L, Count = 50 },
            "/messages", out var path, out _);

        path.Should().Contain("chat_id=5")
            .And.Contain("from=1000")
            .And.Contain("to=2000")
            .And.Contain("count=50");
    }

    // ─── QueryParam: enum → snake_case ───────────────────────────────────────

    [Fact]
    public void EnumQueryParam_ConvertedToSnakeCase()
    {
        ApiRequestBinder.Bind(
            new GetUploadUrlParams { Type = UploadType.Image },
            "/uploads", out var path, out _);

        path.Should().Be("/uploads?type=image");
    }

    [Theory]
    [InlineData(UploadType.Image, "image")]
    [InlineData(UploadType.Video, "video")]
    [InlineData(UploadType.Audio, "audio")]
    [InlineData(UploadType.File,  "file")]
    public void UploadType_AllValues_CorrectSnakeCase(UploadType type, string expected)
    {
        ApiRequestBinder.Bind(new GetUploadUrlParams { Type = type }, "/uploads", out var path, out _);

        path.Should().Contain($"type={expected}");
    }

    // ─── QueryParam: bool → lowercase ────────────────────────────────────────

    [Fact]
    public void BoolQueryParam_True_IsLowercaseTrue()
    {
        ApiRequestBinder.Bind(
            new RemoveChatMemberParams { UserId = 1L, Block = true },
            "/members", out var path, out _);

        path.Should().Contain("block=true");
    }

    [Fact]
    public void BoolQueryParam_False_IsLowercaseFalse()
    {
        ApiRequestBinder.Bind(
            new RemoveChatMemberParams { UserId = 1L, Block = false },
            "/members", out var path, out _);

        path.Should().Contain("block=false");
    }

    [Fact]
    public void NullBoolQueryParam_Skipped()
    {
        ApiRequestBinder.Bind(
            new RemoveChatMemberParams { UserId = 1L, Block = null },
            "/members", out var path, out _);

        path.Should().NotContain("block=");
    }

    // ─── QueryParam: string → Uri.EscapeDataString ───────────────────────────

    [Fact]
    public void StringQueryParam_UrlEncoded()
    {
        ApiRequestBinder.Bind(
            new UnsubscribeParams { Url = "https://example.com/hook?x=1" },
            "/subscriptions", out var path, out _);

        path.Should().Contain("url=https%3A%2F%2Fexample.com%2Fhook%3Fx%3D1");
    }

    [Fact]
    public void PlainStringQueryParam_NoExtraEncoding()
    {
        ApiRequestBinder.Bind(
            new UnsubscribeParams { Url = "https://example.com/hook" },
            "/subscriptions", out var path, out _);

        path.Should().Contain("url=https%3A%2F%2Fexample.com%2Fhook");
    }

    // ─── QueryParam: long[] → comma-separated ────────────────────────────────

    [Fact]
    public void LongArrayQueryParam_CommaSeparated()
    {
        ApiRequestBinder.Bind(
            new GetChatMembersParams { UserIds = new[] { 1L, 2L, 3L } },
            "/members", out var path, out _);

        path.Should().Contain("user_ids=1,2,3");
    }

    [Fact]
    public void SingleElementLongArray_NoComa()
    {
        ApiRequestBinder.Bind(
            new GetChatMembersParams { UserIds = new[] { 42L } },
            "/members", out var path, out _);

        path.Should().Contain("user_ids=42");
    }

    [Fact]
    public void EmptyLongArray_Skipped()
    {
        ApiRequestBinder.Bind(
            new GetChatMembersParams { UserIds = Array.Empty<long>() },
            "/members", out var path, out _);

        path.Should().Be("/members");
    }

    // ─── QueryParam: enum[] → snake_case + comma ─────────────────────────────

    [Fact]
    public void EnumArrayQueryParam_SnakeCaseCommaSeparated()
    {
        ApiRequestBinder.Bind(
            new GetUpdatesParams { UpdateTypes = new[] { Types.UpdateType.MessageCreated, Types.UpdateType.BotAdded } },
            "/updates", out var path, out _);

        path.Should().Contain("types=message_created,bot_added");
    }

    [Fact]
    public void EnumArray_ExplicitAttributeName_UsedInsteadOfPropertyName()
    {
        // UpdateTypes имеет [QueryParam("types")] — не "update_types"
        ApiRequestBinder.Bind(
            new GetUpdatesParams { UpdateTypes = new[] { Types.UpdateType.MessageCreated } },
            "/updates", out var path, out _);

        path.Should().Contain("types=");
        path.Should().NotContain("update_types=");
    }

    // ─── QueryParam: string[] → Uri.EscapeDataString per element ─────────────

    [Fact]
    public void StringArrayQueryParam_EachElementEncoded()
    {
        ApiRequestBinder.Bind(
            new GetMessagesByIdsParams { MessageIds = new[] { "mid/1", "mid/2" } },
            "/messages", out var path, out _);

        path.Should().Contain("message_ids=mid%2F1,mid%2F2");
    }

    [Fact]
    public void EmptyStringArray_Skipped()
    {
        ApiRequestBinder.Bind(
            new GetMessagesByIdsParams { MessageIds = Array.Empty<string>() },
            "/messages", out var path, out _);

        path.Should().Be("/messages");
    }

    // ─── BodyParam → Dictionary<string,object?> ──────────────────────────────

    [Fact]
    public void BodyParam_BuildsDictionary()
    {
        ApiRequestBinder.Bind(
            new SendActionParams { Action = SenderAction.TypingOn },
            "/actions", out _, out var body);

        var dict = body.Should().BeOfType<Dictionary<string, object?>>().Subject;
        dict.Should().ContainKey("action");
        dict["action"].Should().Be(SenderAction.TypingOn);
    }

    [Fact]
    public void MultipleBodyParams_AllInDictionary()
    {
        ApiRequestBinder.Bind(
            new PinMessageParams { MessageId = "msg42", Notify = true },
            "/pin", out _, out var body);

        var dict = body.Should().BeOfType<Dictionary<string, object?>>().Subject;
        dict.Should().ContainKey("message_id").And.ContainKey("notify");
        dict["message_id"].Should().Be("msg42");
        dict["notify"].Should().Be(true);
    }

    [Fact]
    public void NullBodyParam_SkippedFromDictionary()
    {
        // Notify = null → не попадает в словарь
        ApiRequestBinder.Bind(
            new PinMessageParams { MessageId = "msg1", Notify = null },
            "/pin", out _, out var body);

        var dict = body.Should().BeOfType<Dictionary<string, object?>>().Subject;
        dict.Should().ContainKey("message_id");
        dict.Should().NotContainKey("notify");
    }

    [Fact]
    public void AllBodyParamsNull_BodyIsNull()
    {
        // Message и Notification — null, CallbackId — QueryParam, не BodyParam
        ApiRequestBinder.Bind(
            new AnswerCallbackParams { CallbackId = "cb1" },
            "/answers", out _, out var body);

        body.Should().BeNull();
    }

    [Fact]
    public void LongArrayBodyParam_StoredAsArray()
    {
        var ids = new[] { 10L, 20L };
        ApiRequestBinder.Bind(
            new AddChatMembersParams { UserIds = ids },
            "/members", out _, out var body);

        var dict = body.Should().BeOfType<Dictionary<string, object?>>().Subject;
        dict.Should().ContainKey("user_ids");
        dict["user_ids"].Should().BeSameAs(ids);
    }

    // ─── Body → прямой объект ────────────────────────────────────────────────

    [Fact]
    public void BodyAttribute_UsedDirectly()
    {
        var msgBody = new DTOs.NewMessageBody { Text = "hello" };
        ApiRequestBinder.Bind(
            new SendMessageParams { ChatId = 42L, Body = msgBody },
            "/messages", out _, out var body);

        body.Should().BeSameAs(msgBody);
    }

    [Fact]
    public void BodyAttribute_NullValue_BodyIsNull()
    {
        ApiRequestBinder.Bind(
            new EditMessageParams { MessageId = "msg1", Body = null },
            "/messages", out _, out var body);

        body.Should().BeNull();
    }

    // ─── QueryParam + Body вместе ─────────────────────────────────────────────

    [Fact]
    public void QueryParam_And_Body_BothHandled()
    {
        var msgBody = new DTOs.NewMessageBody { Text = "test" };
        ApiRequestBinder.Bind(
            new SendMessageParams { ChatId = 7L, DisableLinkPreview = true, Body = msgBody },
            "/messages", out var path, out var body);

        path.Should().Contain("chat_id=7")
            .And.Contain("disable_link_preview=true");
        body.Should().BeSameAs(msgBody);
    }

    [Fact]
    public void QueryParam_And_BodyParam_BothHandled()
    {
        ApiRequestBinder.Bind(
            new AnswerCallbackParams { CallbackId = "cb1", Notification = "ok" },
            "/answers", out var path, out var body);

        path.Should().Contain("callback_id=cb1");
        var dict = body.Should().BeOfType<Dictionary<string, object?>>().Subject;
        dict["notification"].Should().Be("ok");
    }

    // ─── Property name → snake_case key ──────────────────────────────────────

    [Fact]
    public void PropertyName_ConvertedToSnakeCaseKey()
    {
        ApiRequestBinder.Bind(
            new GetMessagesByChatParams { ChatId = 1L },
            "/messages", out var path, out _);

        // ChatId → chat_id
        path.Should().Contain("chat_id=1");
        path.Should().NotContain("ChatId=");
    }
}
