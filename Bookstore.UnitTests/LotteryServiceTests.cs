using AutoFixture;
using Bookstore.Books;
using FluentAssertions;
using Moq;

namespace Bookstore.UnitTests;

public class LotteryServiceTests
{
    private readonly LotteryService _lotteryService;
    private readonly Mock<IBookClient> _bookClient = new();
    private readonly Fixture _fixture = new();

    public LotteryServiceTests()
    {
        _lotteryService = new LotteryService(_bookClient.Object);
    }

    [Fact]
    public async Task When_Lottery_Number_Is_123456789_Then_Returns_True()
    {
        var copyrightedBooks = _fixture.Build<Book>()
            .With(x => x.Copyright, true)
            .CreateMany();
        var nonCopyrightedBook = _fixture.Build<Book>()
            .With(x => x.Copyright, false)
            .Create();
        var allBooks = new List<Book>(copyrightedBooks) { nonCopyrightedBook };

        _bookClient
            .Setup(x => x.ProcessPurchasedBooks(It.IsAny<IEnumerable<Book>>()))
            .ReturnsAsync(123456789);

        var userWon = await _lotteryService.CheckIfUserWon(allBooks);

        userWon.Should().BeTrue();
    }

    [Fact]
    public async Task When_Lottery_Number_Is_Not_123456789_Then_Returns_False()
    {
        var copyrightedBooks = _fixture.Build<Book>()
            .With(x => x.Copyright, true)
            .CreateMany();
        var nonCopyrightedBook = _fixture.Build<Book>()
            .With(x => x.Copyright, false)
            .Create();
        var allBooks = new List<Book>(copyrightedBooks) { nonCopyrightedBook };

        _bookClient
            .Setup(x => x.ProcessPurchasedBooks(It.IsAny<IEnumerable<Book>>()))
            .ReturnsAsync(1);

        var userWon = await _lotteryService.CheckIfUserWon(allBooks);

        userWon.Should().BeFalse();
    }
}

//_bookClient
//    .Setup(x => x.ProcessPurchasedBooks(ItIs.EquivalentTo(copyrightedBooks)))
//    .ReturnsAsync(123456789);