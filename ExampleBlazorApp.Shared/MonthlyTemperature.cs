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

    public static List<MonthlyTemperature> CreateListForCalendarYear() =>
        new()
        {
            new MonthlyTemperature { Month = 1 },
            new MonthlyTemperature { Month = 2 },
            new MonthlyTemperature { Month = 3 },
            new MonthlyTemperature { Month = 4 },
            new MonthlyTemperature { Month = 5 },
            new MonthlyTemperature { Month = 6 },
            new MonthlyTemperature { Month = 7 },
            new MonthlyTemperature { Month = 8 },
            new MonthlyTemperature { Month = 9 },
            new MonthlyTemperature { Month = 10 },
            new MonthlyTemperature { Month = 11 },
            new MonthlyTemperature { Month = 12 },
        };

    public MonthlyTemperature Clone() =>
        new()
        {
            City = City,
            Month = Month,
            AverageHigh = AverageHigh,
            AverageLow = AverageLow,
            StandardDeviation = StandardDeviation,
        };
}
