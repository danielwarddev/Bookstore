using Bookstore.Books;

namespace Bookstore;

public interface ILotteryService
{
    Task<bool> CheckIfUserWon(IEnumerable<Book> booksPurchased);
}

public class LotteryService
{
    private readonly IBookClient _bookClient;

    public LotteryService(IBookClient bookClient)
    {
        _bookClient = bookClient;
    }

    public async Task<bool> CheckIfUserWon(IEnumerable<Book> booksPurchased)
    {
        var lotteryNumber = await _bookClient.ProcessPurchasedBooks(booksPurchased);
        return lotteryNumber == 123456789;
    }
}
