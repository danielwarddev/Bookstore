namespace Bookstore;

public enum Month
{
    January = 1, February, March, April, May, June, July, August, September, October, November, December
}

public static class MonthExtensions
{
    public static (DateOnly Startdate, DateOnly EndDate) GetDateRange(this Month month)
    {
        var monthValue = (int)month;
        var currentYear = DateTime.Now.Year;
        var daysInMonth = DateTime.DaysInMonth(currentYear, monthValue);

        var startDate = new DateOnly(currentYear, monthValue, 1);
        var endDate = new DateOnly(currentYear, monthValue, daysInMonth);

        return (startDate, endDate);
    }
}