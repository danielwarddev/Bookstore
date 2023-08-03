using Bookstore.FileParser;
using FluentAssertions;

namespace Bookstore.UnitTests.FileParser;

public class FileLineParserUnitTests
{
    private readonly FileLineParser _parser = new();

    public static TheoryData<string, LineData> HappyTestCases = new()
    {
        {
            "[Very Cool Book] Bob, Daniel Ward, Julia Childs; 2003; en, es, fn; Drama/Fiction/History",
            new LineData
            (
                "Very Cool Book",
                new string[] { "Bob", "Daniel Ward", "Julia Childs" },
                2003,
                new string[] { "en", "es", "fn" },
                new string[] { "Drama", "Fiction", "History" }
            )
        },
        {
            "[      Yet Another Book     ] Person, George Last-Name, King Charles Jr.; 100; ; ",
            new LineData
            (
                "Yet Another Book",
                new string[] { "Person", "George Last-Name", "King Charles Jr." },
                100,
                new string[] { string.Empty },
                new string[] { string.Empty }
            )
        }
    };

    public static TheoryData<string> SadTestCases = new()
    {
        "[Book name without a closing bracket Bob, Daniel Ward, Julia Childs; 2003; en, es, fn; Drama/Fiction/History",
        "[Line with ] too many; semicolons; Bob, Daniel Ward, Julia Childs; 2003; en, es, fn; Drama/Fiction/History",
        "Line missing data",
        ""
    };

    [Theory]
    [MemberData(nameof(HappyTestCases))]
    public void Line_Is_Parsed_When_Formatted_Correctly(string line, LineData expectedLineData)
    {
        var actualLineData = _parser.ParseLine(line);
        actualLineData.Should().BeEquivalentTo(expectedLineData);
    }

    [Theory]
    [MemberData(nameof(SadTestCases))]
    public void Throws_Exception_When_Line_Is_Malformatted(string line)
    {
        var func = () => _parser.ParseLine(line);
        func.Should().Throw<Exception>();
    }
}
