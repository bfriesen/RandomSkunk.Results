using ExampleBlazorApp.Server.Repositories;
using ExampleBlazorApp.Shared;
using RandomSkunk.Results;

namespace ExampleBlazorApp.Server.Services;

public class WeatherForecastSimulator : IWeatherForecastSimulator
{
    private readonly IWeatherRepository _weatherRepository;
    private readonly Random _random;

    public WeatherForecastSimulator(IWeatherRepository weatherRepository)
        : this(weatherRepository, new())
    {
    }

    public WeatherForecastSimulator(IWeatherRepository weatherRepository, Random random)
    {
        _weatherRepository = weatherRepository;
        _random = random;
    }

    public async Task<Maybe<IReadOnlyList<WeatherForecast>>> GetFiveDayForecast(string city)
    {
        // Get the monthly temperature of the city and the current month using our WeatherRepository.
        Maybe<MonthlyTemperature> monthlyTemperatureResult = await _weatherRepository.GetMonthlyTemperature(city, DateTime.Now.Month);

        // Convert the Maybe<MonthlyTemperature> into a Maybe<IReadOnlyList<WeatherForecast>>
        // using the Select method. This works very similar to the Select extension method from LINQ.
        return monthlyTemperatureResult.Select(GenerateFiveDayForecast);
    }

    private IReadOnlyList<WeatherForecast> GenerateFiveDayForecast(MonthlyTemperature monthlyTemperature)
    {
        var fiveDay = new WeatherForecast[5];
        var date = DateTime.Today;

        for (int i = 0; i < 5; i++, date = date.AddDays(1))
        {
            var low = NextGaussian(monthlyTemperature.AverageLow, monthlyTemperature.StandardDeviation);
            var high = NextGaussian(monthlyTemperature.AverageHigh, monthlyTemperature.StandardDeviation);
            fiveDay[i] = new WeatherForecast { LowF = low, HighF = high, Date = date };
        }

        return fiveDay;
    }

    private double NextGaussian(double mean, double standardDeviation)
    {
        double v1, v2, s;
        do
        {
            v1 = (2.0 * _random.NextDouble()) - 1.0;
            v2 = (2.0 * _random.NextDouble()) - 1.0;
            s = (v1 * v1) + (v2 * v2);
        } while (s >= 1.0 || s == 0);
        s = Math.Sqrt((-2.0 * Math.Log(s)) / s);

        return (standardDeviation * v1 * s) + mean;
    }
}
