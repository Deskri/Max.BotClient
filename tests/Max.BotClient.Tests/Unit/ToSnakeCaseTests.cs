using FluentAssertions;
using Max.BotClient;
using Xunit;

namespace Max.BotClient.Tests.Unit;

public class ToSnakeCaseTests
{
    [Theory]
    [InlineData("TypingOn",            "typing_on")]
    [InlineData("MessageId",           "message_id")]
    [InlineData("DisableLinkPreview",  "disable_link_preview")]
    [InlineData("UpdateTypes",         "update_types")]
    [InlineData("ChatId",              "chat_id")]
    [InlineData("url",                 "url")]
    [InlineData("A",                   "a")]
    [InlineData("ABC",                 "a_b_c")]
    [InlineData("UserIds",             "user_ids")]
    [InlineData("SendingPhoto",        "sending_photo")]
    public void ToSnakeCase_ConvertsCorrectly(string input, string expected) =>
        input.ToSnakeCase().Should().Be(expected);
}
