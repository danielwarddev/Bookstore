using System.Text.Json.Serialization;

namespace Bookstore.Books;

public record Book
(
    int Id,
    string Title,
    Author[] Authors,
    string[] Translators,
    string[] Subjects,
    string[] Bookshelves,
    string[] Languages,
    bool Copyright
);

public record Author
(
    string Name,
    [property: JsonPropertyName("birth_year")]
    int BirthYear, 
    [property: JsonPropertyName("death_year")]
    int DeathYear
);
