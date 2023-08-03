using Bookstore.Database;
using Bookstore.Database.Entities;

namespace Bookstore.Books;

public interface IBookService
{
    public Task LikeBook(int bookId, int userId);
}

public class BookService : IBookService
{
    private readonly BookstoreContext _context;

    public BookService(BookstoreContext context)
    {
        _context = context;
    }

    public async Task LikeBook(int bookId, int userId)
    {
        var existingBookLike = _context.BookLikes.FirstOrDefault(x => x.GutendexBookId == bookId && x.UserId == userId);
        if (existingBookLike != null)
        {
            return;
        }

        await _context.BookLikes.AddAsync(new BookLike()
        {
            GutendexBookId = bookId,
            UserId = userId
        });
        _context.SaveChanges();
    }
}
