using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Max.BotClient.Tests.Integration.Helpers;

internal sealed class FakeHttpHandler : HttpMessageHandler
{
    private readonly string _json;

    public HttpRequestMessage? LastRequest { get; private set; }
    public string? CapturedBody { get; private set; }

    public FakeHttpHandler(string json) => _json = json;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        CapturedBody = request.Content != null
            ? await request.Content.ReadAsStringAsync()
            : null;

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(_json, Encoding.UTF8, "application/json")
        };
    }

    public static (BotClient client, FakeHttpHandler handler) Create(string json)
    {
        var handler = new FakeHttpHandler(json);
        var httpClient = new HttpClient(handler);
        var options = new BotClientOptions("test-token") { RetryCount = 0 };
        var client = new BotClient(options, httpClient);
        return (client, handler);
    }
}