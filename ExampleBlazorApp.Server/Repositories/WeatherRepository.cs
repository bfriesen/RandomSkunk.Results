using ExampleBlazorApp.Shared;
using Microsoft.Data.Sqlite;
using RandomSkunk.Results;
using RandomSkunk.Results.Dapper;

namespace ExampleBlazorApp.Server.Repositories;

public class WeatherRepository : IWeatherRepository
{
    private readonly string _connectionString;

    public WeatherRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("WeatherData");
    }

    public async Task<Maybe<MonthlyTemperature>> GetAverageTemperature(string city, int month)
    {
        const string sql = @"
SELECT
    City,
    Month,
    AverageHigh,
    AverageLow,
    StandardDeviation
FROM WeatherData
WHERE City = @city AND Month = @month;";

        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        return await connection.TryQuerySingleOrNoneAsync<MonthlyTemperature>(sql, new { city, month });
    }

    public async Task<Result<IReadOnlyList<WeatherProfile>>> GetWeatherProfiles()
    {
        const string sql = @"
SELECT
    City,
    Month,
    AverageHigh,
    AverageLow,
    StandardDeviation
FROM WeatherData";

        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        return await connection.TryQueryAsync<MonthlyTemperature>(sql)
            .Map(value => (IReadOnlyList<WeatherProfile>)value.GroupBy(t => t.City).Select(g =>
                new WeatherProfile { City = g.Key, MonthlyTemperatures = g.OrderBy(x => x.Month).ToList() }).ToList());
    }

    public Task<Result> AddWeatherProfile(WeatherProfile weatherProfile)
    {
        const string sql = @"
INSERT INTO WeatherData (
    City,
    Month,
    AverageHigh,
    AverageLow,
    StandardDeviation)
VALUES (
    @City,
    @Month,
    @AverageHigh,
    @AverageLow,
    @StandardDeviation);";

        return UpsertWeatherProfile(weatherProfile, sql);
    }

    public Task<Result> EditWeatherProfile(WeatherProfile weatherProfile)
    {
        const string sql = @"
UPDATE WeatherData
SET
    AverageHigh = @AverageHigh,
    AverageLow = @AverageLow,
    StandardDeviation = @StandardDeviation
WHERE City = @City AND Month = @Month;";

        return UpsertWeatherProfile(weatherProfile, sql);
    }

    private async Task<Result> UpsertWeatherProfile(WeatherProfile weatherProfile, string sql)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();

        var result = Result.Success();
        foreach (var monthlyTemperature in weatherProfile.MonthlyTemperatures)
        {
            var param = new
            {
                weatherProfile.City,
                monthlyTemperature.Month,
                monthlyTemperature.AverageHigh,
                monthlyTemperature.AverageLow,
                monthlyTemperature.StandardDeviation,
            };

            result = await result.AndAlsoAsync(
                () => connection.TryExecuteAsync(sql, param, transaction)
                    .CrossMap(affectedRowCount =>
                        affectedRowCount == 1
                            ? Result.Success()
                            : Result.Fail($"Expected 1 row to be affected, but was {affectedRowCount}.")));

            if (result.IsFail)
            {
                await transaction.RollbackAsync();
                return result;
            }
        }

        await transaction.CommitAsync();
        return result;
    }
}
