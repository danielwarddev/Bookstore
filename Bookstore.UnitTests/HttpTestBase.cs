using Moq;

namespace Bookstore.UnitTests;

public abstract class HttpTestBase
{
    protected readonly string BaseAddress = "https://www.google.com";
    protected Mock<HttpMessageHandler> HttpMessageHandler { get; }
    protected HttpClient HttpClient { get; }

    protected HttpTestBase()
    {
        HttpMessageHandler = new Mock<HttpMessageHandler>();
        HttpClient = new HttpClient(HttpMessageHandler.Object)
        {
            BaseAddress = new Uri(BaseAddress)
        };
    }
}
