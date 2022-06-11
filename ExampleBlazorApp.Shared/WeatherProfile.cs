namespace ExampleBlazorApp.Shared;

public class WeatherProfile
{
    public string City { get; init; } = null!;

    public IReadOnlyList<MonthlyTemperature> MonthlyTemperatures { get; init; } = null!;
}
