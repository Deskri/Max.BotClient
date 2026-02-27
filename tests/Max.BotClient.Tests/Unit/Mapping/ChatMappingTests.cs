using FluentAssertions;
using Max.BotClient.DTOs;
using Max.BotClient.Mapping;
using Xunit;

namespace Max.BotClient.Tests.Unit.Mapping;

public class ChatMappingTests
{
    // ─── null → null ─────────────────────────────────────────────────────────

    [Fact]
    public void NullChat_ReturnsNull() =>
        ((DTOs.Chat?)null).ToChat().Should().BeNull();

    // ─── Scalar fields ───────────────────────────────────────────────────────

    [Fact]
    public void Chat_ScalarFields_Mapped()
    {
        var dto = new DTOs.Chat
        {
            ChatId = 10L,
            Type = DTOs.ChatType.Chat,
            Status = DTOs.ChatStatus.Active,
            Title = "Test Chat",
            LastEventTime = 1_700_000_000L,
            ParticipantsCount = 5,
            OwnerId = 42L,
            IsPublic = true,
            Link = "https://t.me/testchat",
            Description = "Test description"
        };

        var result = dto.ToChat()!;

        result.ChatId.Should().Be(10L);
        result.Type.Should().Be(Types.ChatType.Chat);
        result.Status.Should().Be(Types.ChatStatus.Active);
        result.Title.Should().Be("Test Chat");
        result.LastEventTime.Should().Be(1_700_000_000L);
        result.ParticipantsCount.Should().Be(5);
        result.OwnerId.Should().Be(42L);
        result.IsPublic.Should().BeTrue();
        result.Link.Should().Be("https://t.me/testchat");
        result.Description.Should().Be("Test description");
    }

    // ─── Icon ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Chat_Icon_Mapped()
    {
        var dto = new DTOs.Chat
        {
            ChatId = 1L,
            Icon = new DTOs.Image { Url = "https://example.com/icon.png" }
        };

        dto.ToChat()!.Icon!.Url.Should().Be("https://example.com/icon.png");
    }

    [Fact]
    public void Chat_NullIcon_IsNull()
    {
        var dto = new DTOs.Chat { ChatId = 1L, Icon = null };

        dto.ToChat()!.Icon.Should().BeNull();
    }

    // ─── ChatMember ───────────────────────────────────────────────────────────

    [Fact]
    public void NullChatMember_ReturnsNull() =>
        ((DTOs.ChatMember?)null).ToChatMember().Should().BeNull();

    [Fact]
    public void ChatMember_AllFields_Mapped()
    {
        var dto = new DTOs.ChatMember
        {
            UserId = 7L,
            FirstName = "Alice",
            LastName = "Smith",
            Username = "alice",
            IsBot = false,
            LastActivityTime = 123L,
            LastAccessTime = 456L,
            IsOwner = true,
            IsAdmin = false,
            JoinTime = 789L,
            Permissions = new[] { DTOs.ChatAdminPermission.Write, DTOs.ChatAdminPermission.PinMessage },
            Alias = "queen"
        };

        var result = dto.ToChatMember()!;

        result.UserId.Should().Be(7L);
        result.FirstName.Should().Be("Alice");
        result.LastName.Should().Be("Smith");
        result.Username.Should().Be("alice");
        result.IsBot.Should().BeFalse();
        result.LastActivityTime.Should().Be(123L);
        result.LastAccessTime.Should().Be(456L);
        result.IsOwner.Should().BeTrue();
        result.IsAdmin.Should().BeFalse();
        result.JoinTime.Should().Be(789L);
        result.Permissions.Should().BeEquivalentTo(new[]
        {
            Types.ChatAdminPermission.Write,
            Types.ChatAdminPermission.PinMessage
        });
        result.Alias.Should().Be("queen");
    }

    [Fact]
    public void ChatMember_NullPermissions_IsNull()
    {
        var dto = new DTOs.ChatMember { UserId = 1L, FirstName = "X", Permissions = null };

        dto.ToChatMember()!.Permissions.Should().BeNull();
    }

    // ─── GetChatsResponse ────────────────────────────────────────────────────

    [Fact]
    public void NullGetChatsResponse_ReturnsNull() =>
        ((DTOs.GetChatsResponse?)null).ToGetChatsResponse().Should().BeNull();

    [Fact]
    public void GetChatsResponse_MapsChatsAndMarker()
    {
        var dto = new DTOs.GetChatsResponse
        {
            Chats = new[] { new DTOs.Chat { ChatId = 5L, Title = "Group" } },
            Marker = 99L
        };

        var result = dto.ToGetChatsResponse()!;

        result.Chats.Should().HaveCount(1);
        result.Chats![0].ChatId.Should().Be(5L);
        result.Marker.Should().Be(99L);
    }

    // ─── GetChatMembersResponse ──────────────────────────────────────────────

    [Fact]
    public void NullGetChatMembersResponse_ReturnsNull() =>
        ((DTOs.GetChatMembersResponse?)null).ToGetChatMembersResponse().Should().BeNull();

    [Fact]
    public void GetChatMembersResponse_MapsMembersAndMarker()
    {
        var dto = new DTOs.GetChatMembersResponse
        {
            Members = new[] { new DTOs.ChatMember { UserId = 3L, FirstName = "Bob" } },
            Marker = 77L
        };

        var result = dto.ToGetChatMembersResponse()!;

        result.Members.Should().HaveCount(1);
        result.Members![0].UserId.Should().Be(3L);
        result.Marker.Should().Be(77L);
    }
}