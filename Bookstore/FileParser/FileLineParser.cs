namespace Bookstore.FileParser;

public interface IFileLineParser
{
    LineData ParseLine(string line);
}


public class FileLineParser
{
    // [Book Title] Author1, Author2First Author2Last, Author3; 2003; en, es, fn; Drama/Fiction/History
    public LineData ParseLine(string line)
    {
        var bookNameStart = line.IndexOf('[') + 1;
        var bookNameEnd = line.IndexOf(']');
        var bookName = line.Substring(bookNameStart, bookNameEnd - bookNameStart).Trim();

        line = line.Substring(bookNameEnd + 1);

        var authorsEnd = line.IndexOf(';');
        var authorsString = line.Substring(0, authorsEnd);
        var authors = authorsString.Split(',').Select(x => x.Trim()).ToArray();

        line = line.Substring(authorsEnd + 1);

        var yearEnd = line.IndexOf(';');
        var yearString = line.Substring(0, yearEnd).Trim();
        var year = int.Parse(yearString);

        line = line.Substring(yearEnd + 1);

        var languagesEnd = line.IndexOf(';');
        var languagesString = line.Substring(0, languagesEnd);
        var languages = languagesString.Split(',').Select(x => x.Trim()).ToArray();

        line = line.Substring(languagesEnd + 1);

        var subjects = line.Split('/').Select(x => x.Trim()).ToArray();

        return new LineData(bookName, authors, year, languages, subjects);
    }
}
