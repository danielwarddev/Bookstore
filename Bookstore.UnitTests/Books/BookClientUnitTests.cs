using AutoFixture;
using Bookstore.Books;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.ComponentModel;
using System.Net;
using System.Text.Json;

namespace Bookstore.UnitTests.Books;

public class BookClientUnitTests : HttpTestBase
{
    private readonly BookClient _bookClient;
    private readonly Fixture _fixture = new();

    public BookClientUnitTests()
    {
        _bookClient = new BookClient(HttpClient);
    }

    [Fact]
    public async Task When_Getting_A_Book_Returns_Successful_Response_Then_Returns_The_Book()
    {
        // 1. Arrange
        var expectedBook = _fixture.Create<Book>();
        var serializedBook = JsonSerializer.Serialize(expectedBook);
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(serializedBook)
        };

        HttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        ).ReturnsAsync(response);

        // 2. Act
        var actualBook = await _bookClient.GetBook(expectedBook.Id);

        // 3. Assert
        expectedBook.Should().BeEquivalentTo(actualBook);
    }

    [Fact]
    public async Task When_Getting_A_Book_Returns_404_Then_Returns_Null()
    {
        var bookId = _fixture.Create<int>();

        HttpMessageHandler
            .SetupSendAsync(HttpMethod.Get, $"{BaseAddress}/books/{bookId}")
            .ReturnsHttpResponseAsync(null, HttpStatusCode.NotFound);

        var result = await _bookClient.GetBook(bookId);

        result.Should().BeNull();
    }

    [Theory, CombinatorialData]
    public async Task When_Extension_And_Size_Are_Valid_Then_Uploads_Photo(
        [CombinatorialValues(".jpg",".jpeg",".png")] string fileExtension,
        [CombinatorialValues(1000, 3000, 10000)] int fileSize)
    {
        var fileName = $"myFile{fileExtension}";
        var fileData = new byte[fileSize];

        HttpMessageHandler
            .SetupSendAsync(HttpMethod.Post, $"{BaseAddress}/photos")
            .ReturnsHttpResponseAsync(null, HttpStatusCode.OK);

        await _bookClient.UploadPhoto(fileName, fileData);
    }

    [Theory, CombinatorialData]
    public async Task When_Extension_Is_Invalid_Then_Throws_Exception(
        [CombinatorialValues(".gif", ".txt", ".bmp")] string fileExtension,
        [CombinatorialValues(1000, 3000, 10000)] int fileSize)
    {
        var fileName = $"myFile{fileExtension}";
        var fileData = new byte[fileSize];

        var func = async () => await _bookClient.UploadPhoto(fileName, fileData);
        await func.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid file extension");
    }

    [Theory, CombinatorialData]
    public async Task When_Size_Is_Over_10kb_Then_Throws_Exception(
    [CombinatorialValues(".jpg", ".jpeg", ".png")] string fileExtension,
    [CombinatorialValues(10001, 20000, 100000)] int fileSize)
    {
        var fileName = $"myFile{fileExtension}";
        var fileData = new byte[fileSize];

        var func = async () => await _bookClient.UploadPhoto(fileName, fileData);
        await func.Should().ThrowAsync<ArgumentException>().WithMessage("File size must be under 10kb");
    }

    [Fact]
    public async Task Calls_Endpoint_With_Date_Range_From_Given_Month()
    {
        var month = GetRandomMonth();
        var dates = month.GetDateRange();
        var expectedBooks = _fixture.CreateMany<Book>();

        HttpMessageHandler
            .SetupSendAsync(HttpMethod.Get, $"{BaseAddress}/books?beginDate={dates.Startdate}&endDate={dates.EndDate}")
            .ReturnsHttpResponseAsync(expectedBooks, HttpStatusCode.OK);

        var actualBooks = await _bookClient.GetBooksAddedInMonth(month);

        actualBooks.Should().BeEquivalentTo(expectedBooks);
        // Assert.Equal(expectedBooks, actualBooks); // doesn't work!
    }

    private Month GetRandomMonth()
    {
        var random = new Random();
        var allMonths = Enum.GetValues(typeof(Month));
        var randomIndex = random.Next(allMonths.Length);

        return (Month) allMonths.GetValue(randomIndex)!;
    }
}
