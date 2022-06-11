namespace ExampleBlazorApp.Shared;

public class WeatherForecast
{
    public DateTime Date { get; init; }

    public double HighC { get; init; }

    public double LowC { get; init; }

    public double HighF { get => 32 + (HighC * 9.0 / 5.0); init => HighC = (value - 32) * 5.0 / 9.0; }

    public double LowF { get => 32 + (LowC * 9.0 / 5.0); init => LowC = (value - 32) * 5.0 / 9.0; }
}
