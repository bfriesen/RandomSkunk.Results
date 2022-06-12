namespace ExampleBlazorApp.Shared;

public class WeatherProfile
{
    public string City { get; set; } = null!;

    public List<MonthlyTemperature> MonthlyTemperatures { get; init; } = new List<MonthlyTemperature>();

    public WeatherProfile Clone() =>
        new()
        {
            City = City,
            MonthlyTemperatures = MonthlyTemperatures.Select(x => x.Clone()).ToList(),
        };
}
