using Bookstore.Books;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController
{
    private readonly IBookClient _bookClient;

    public BookController(IBookClient bookClient)
    {
        _bookClient = bookClient;
    }

    [HttpGet("{id:int}")]
    public async Task<Book?> GetBook([FromRoute] int id)
    {
        return await _bookClient.GetBook(id);
    }
}
