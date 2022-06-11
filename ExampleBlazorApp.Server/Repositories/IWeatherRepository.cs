using ExampleBlazorApp.Shared;
using RandomSkunk.Results;

namespace ExampleBlazorApp.Server.Repositories
{
    public interface IWeatherRepository
    {
        Task<Maybe<MonthlyTemperature>> GetAverageTemperature(string city, int month);

        Task<Result<IReadOnlyList<WeatherProfile>>> GetWeatherProfiles();

        Task<Result> AddWeatherProfile(WeatherProfile weatherProfile);

        Task<Result> EditWeatherProfile(WeatherProfile weatherProfile);
    }
}
