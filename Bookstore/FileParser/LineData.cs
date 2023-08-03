namespace Bookstore.FileParser;

public record LineData(string BookName, string[] Authors, int YearPrinted, string[] Languages, string[] Subjects);