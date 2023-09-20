namespace BlazorWasmClient.Shared;

public static class DateTimeExtension
{
    public static DateTime YearsAgo(this DateTime date, int years) => date.AddYears(-1 * years);
    
    public static DateTime YearAgo(this DateTime date) => YearsAgo(date, 1);

}