using AutoFixture;
using Bookstore.Books;
using Bookstore.Database.Entities;
using FluentAssertions;

namespace Bookstore.IntegrationTests;

public class BookServiceIntTests : IntegrationTestBase
{
    private BookService _bookService;
    private Fixture _fixture = new();

    public BookServiceIntTests(IntegrationTestFactory factory) : base(factory)
    {
        _bookService = new BookService(DbContext);
    }

    [Fact]
    public async Task When_BookLike_Does_Not_Exist_In_Database_Then_Adds_It()
    {
        var gutendexId = _fixture.Create<int>();
        var userId = _fixture.Create<int>();

        await _bookService.LikeBook(gutendexId, userId);

        var allBookLikes = DbContext.BookLikes.ToList();
        allBookLikes.Count.Should().Be(1);
        allBookLikes[0].Should().BeEquivalentTo(new BookLike()
        {
            GutendexBookId = gutendexId,
            UserId = userId
        }, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task When_BookLike_Does_Exist_In_Database_Then_Does_Nothing()
    {
        var gutendexId = _fixture.Create<int>();
        var userId = _fixture.Create<int>();

        await AddAsync(new BookLike()
        {
            GutendexBookId = gutendexId,
            UserId = userId
        });

        await _bookService.LikeBook(gutendexId, userId);

        var allBookLikes = DbContext.BookLikes.ToList();
        allBookLikes.Count.Should().Be(1);
    }
}
