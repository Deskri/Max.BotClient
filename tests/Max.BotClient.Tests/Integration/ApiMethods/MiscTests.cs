using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Max.BotClient.Tests.Integration.Helpers;
using Max.BotClient.Types;
using Xunit;

namespace Max.BotClient.Tests.Integration.ApiMethods;

public class MiscTests
{
    // ─── GetMe ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetMe_SendsGetToMePath()
    {
        var (client, handler) = FakeHttpHandler.Create(
            """{"user_id":1,"first_name":"TestBot","is_bot":true}""");

        await client.GetMe();

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/me");
    }

    // ─── GetUploadUrl ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetUploadUrl_SendsPostToUploadsPath()
    {
        var (client, handler) = FakeHttpHandler.Create("""{"url":"https://upload.example.com"}""");

        await client.GetUploadUrl(UploadType.Image);

        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/uploads");
    }

    [Theory]
    [InlineData(UploadType.Image, "image")]
    [InlineData(UploadType.Video, "video")]
    [InlineData(UploadType.Audio, "audio")]
    [InlineData(UploadType.File,  "file")]
    public async Task GetUploadUrl_TypeSnakeCaseInQuery(UploadType type, string expected)
    {
        var (client, handler) = FakeHttpHandler.Create("""{"url":"https://upload.example.com"}""");

        await client.GetUploadUrl(type);

        handler.LastRequest!.RequestUri!.Query.Should().Contain($"type={expected}");
    }

    // ─── GetVideo ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetVideo_SendsGetToVideoPath()
    {
        var (client, handler) = FakeHttpHandler.Create("{}");

        await client.GetVideo("token123");

        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/videos/token123");
    }

    [Fact]
    public async Task GetVideo_SpecialCharsInToken_UriEncoded()
    {
        var (client, handler) = FakeHttpHandler.Create("{}");

        await client.GetVideo("tok/en");

        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be("/videos/tok%2Fen");
    }
}