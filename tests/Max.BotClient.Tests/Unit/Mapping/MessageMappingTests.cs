using FluentAssertions;
using Max.BotClient.DTOs;
using Max.BotClient.Mapping;
using Xunit;

namespace Max.BotClient.Tests.Unit.Mapping;

public class MessageMappingTests
{
    // ─── null → null ─────────────────────────────────────────────────────────

    [Fact]
    public void NullDto_ReturnsNull() =>
        ((DTOs.Message?)null).ToMessage().Should().BeNull();

    // ─── Базовые поля Body ────────────────────────────────────────────────────

    [Fact]
    public void Body_MapsTextMidSeq()
    {
        var dto = new DTOs.Message
        {
            Recipient = new Recipient(),
            Body = new MessageBody { Mid = "mid123", Seq = 7, Text = "Hello" }
        };

        var result = dto.ToMessage()!;

        result.Text.Should().Be("Hello");
        result.Mid.Should().Be("mid123");
        result.Seq.Should().Be(7L);
    }

    [Fact]
    public void NullBody_PropertiesAreNull()
    {
        var dto = new DTOs.Message { Recipient = new Recipient(), Body = null };

        var result = dto.ToMessage()!;

        result.Text.Should().BeNull();
        result.Mid.Should().BeNull();
        result.Seq.Should().BeNull();
    }

    // ─── Timestamp и Url ──────────────────────────────────────────────────────

    [Fact]
    public void Timestamp_And_Url_Mapped()
    {
        var dto = new DTOs.Message
        {
            Recipient = new Recipient(),
            Body = new MessageBody { Mid = "m1", Seq = 1 },
            Timestamp = 1_700_000_000L,
            Url = "https://example.com/post/1"
        };

        var result = dto.ToMessage()!;

        result.Timestamp.Should().Be(1_700_000_000L);
        result.Url.Should().Be("https://example.com/post/1");
    }

    // ─── Sender ───────────────────────────────────────────────────────────────

    [Fact]
    public void Sender_MapsToTypesUser()
    {
        var dto = new DTOs.Message
        {
            Recipient = new Recipient(),
            Body = new MessageBody { Mid = "m1", Seq = 1 },
            Sender = new User { UserId = 42L, FirstName = "John", LastName = "Doe", Username = "jdoe", IsBot = false }
        };

        var result = dto.ToMessage()!;

        result.Sender!.UserId.Should().Be(42L);
        result.Sender.FirstName.Should().Be("John");
        result.Sender.LastName.Should().Be("Doe");
        result.Sender.Username.Should().Be("jdoe");
        result.Sender.IsBot.Should().BeFalse();
    }

    [Fact]
    public void NullSender_IsNull()
    {
        var dto = new DTOs.Message
        {
            Recipient = new Recipient(),
            Body = new MessageBody { Mid = "m1", Seq = 1 },
            Sender = null
        };

        dto.ToMessage()!.Sender.Should().BeNull();
    }

    // ─── Recipient ────────────────────────────────────────────────────────────

    [Fact]
    public void Recipient_ChatId_Mapped()
    {
        var dto = new DTOs.Message
        {
            Recipient = new Recipient { ChatId = 100L },
            Body = new MessageBody { Mid = "m1", Seq = 1 }
        };

        var result = dto.ToMessage()!;

        result.RecipientChatId.Should().Be(100L);
        result.RecipientUserId.Should().BeNull();
    }

    [Fact]
    public void Recipient_UserId_Mapped()
    {
        var dto = new DTOs.Message
        {
            Recipient = new Recipient { UserId = 55L },
            Body = new MessageBody { Mid = "m1", Seq = 1 }
        };

        dto.ToMessage()!.RecipientUserId.Should().Be(55L);
    }

    // ─── LinkedMessage (Link) ─────────────────────────────────────────────────

    [Fact]
    public void Link_AllFields_Mapped()
    {
        var dto = new DTOs.Message
        {
            Recipient = new Recipient(),
            Body = new MessageBody { Mid = "m1", Seq = 1 },
            Link = new LinkedMessage
            {
                Type = MessageLinkType.Reply,
                Sender = new User { UserId = 5L, FirstName = "Alice" },
                ChatId = 200L,
                Message = new MessageBody { Mid = "linked123", Seq = 2, Text = "Original" }
            }
        };

        var result = dto.ToMessage()!;

        result.LinkType.Should().Be(Types.MessageLinkType.Reply);
        result.LinkSender!.UserId.Should().Be(5L);
        result.LinkChatId.Should().Be(200L);
        result.LinkMid.Should().Be("linked123");
        result.LinkSeq.Should().Be(2L);
        result.LinkText.Should().Be("Original");
    }

    [Fact]
    public void NullLink_AllLinkPropertiesNull()
    {
        var dto = new DTOs.Message
        {
            Recipient = new Recipient(),
            Body = new MessageBody { Mid = "m1", Seq = 1 },
            Link = null
        };

        var result = dto.ToMessage()!;

        result.LinkType.Should().BeNull();
        result.LinkSender.Should().BeNull();
        result.LinkChatId.Should().BeNull();
        result.LinkMid.Should().BeNull();
    }

    // ─── Stat.Views ───────────────────────────────────────────────────────────

    [Fact]
    public void Stat_Views_Mapped()
    {
        var dto = new DTOs.Message
        {
            Recipient = new Recipient(),
            Body = new MessageBody { Mid = "m1", Seq = 1 },
            Stat = new MessageStat { Views = 42 }
        };

        dto.ToMessage()!.Views.Should().Be(42);
    }

    [Fact]
    public void NullStat_ViewsIsNull()
    {
        var dto = new DTOs.Message
        {
            Recipient = new Recipient(),
            Body = new MessageBody { Mid = "m1", Seq = 1 },
            Stat = null
        };

        dto.ToMessage()!.Views.Should().BeNull();
    }

    // ─── ToMessages (массив) ──────────────────────────────────────────────────

    [Fact]
    public void ToMessages_NullArray_ReturnsNull() =>
        ((DTOs.Message[]?)null).ToMessages().Should().BeNull();

    [Fact]
    public void ToMessages_MapsAllElements()
    {
        var dtos = new[]
        {
            new DTOs.Message { Recipient = new Recipient(), Body = new MessageBody { Mid = "m1", Seq = 1, Text = "A" } },
            new DTOs.Message { Recipient = new Recipient(), Body = new MessageBody { Mid = "m2", Seq = 2, Text = "B" } }
        };

        var result = dtos.ToMessages()!;

        result.Should().HaveCount(2);
        result[0].Text.Should().Be("A");
        result[1].Text.Should().Be("B");
    }
}
