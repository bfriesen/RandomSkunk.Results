namespace ExampleBlazorApp.Shared;

public class MonthlyTemperature
{
    public string City { get; set; } = null!;

    public int Month { get; set; }

    public double AverageHigh { get; set; }

    public double AverageLow { get; set; }

    public double StandardDeviation { get; set; }

    public string MonthName =>
        Month switch
        {
            1 => "January",
            2 => "February",
            3 => "March",
            4 => "April",
            5 => "May",
            6 => "June",
            7 => "July",
            8 => "August",
            9 => "September",
            10 => "October",
            11 => "November",
            12 => "December",
            _ => null!,
        };
}
