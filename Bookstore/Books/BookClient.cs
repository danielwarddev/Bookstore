using System.Net;

namespace Bookstore.Books;

public interface IBookClient
{
    Task<Book?> GetBook(int id);
    Task UploadPhoto(string fileName, byte[] data);
    Task<Book[]> GetBooksAddedInMonth(Month month);
    Task<int> ProcessPurchasedBooks(IEnumerable<Book> books);
}

public class BookClient : IBookClient
{
    public static readonly string[] ValidFileExtensions = new string[] { ".jpg", ".jpeg", ".png" };
    private readonly HttpClient _httpClient;

    public BookClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Book?> GetBook(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Book>($"/books/{id}");
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            throw;
        }
    }

    public async Task UploadPhoto(string fileName, byte[] data)
    {
        if (data.Length > 10000)
        {
            throw new ArgumentException("File size must be under 10kb");
        }

        var fileExtension = fileName.Substring(fileName.LastIndexOf('.'));
        if (!ValidFileExtensions.Contains(fileExtension))
        {
            throw new ArgumentException("Invalid file extension");
        }

        var photo = new BookPhoto(fileName, data);
        await _httpClient.PostAsJsonAsync("/photos", photo);
    }

    public async Task<Book[]> GetBooksAddedInMonth(Month month)
    {
        var dates = month.GetDateRange();
        return (await _httpClient.GetFromJsonAsync<Book[]>($"/books?beginDate={dates.Startdate}&endDate={dates.EndDate}"))!;
    }

    public async Task <int>ProcessPurchasedBooks(IEnumerable<Book> books)
    {
        var bookIds = books.Select(x => x.Id);
        return (await _httpClient.GetFromJsonAsync<int>($"/books?bookIds={bookIds}"))!;
    }
}
