using ExampleBlazorApp.Shared;
using Microsoft.Data.Sqlite;
using RandomSkunk.Results;
using RandomSkunk.Results.Dapper;
using System.Data.Common;

namespace ExampleBlazorApp.Server.Repositories;

public class WeatherRepository : IWeatherRepository
{
    private readonly string _connectionString;

    public WeatherRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("WeatherData");
    }

    public async Task<Maybe<MonthlyTemperature>> GetMonthlyTemperature(string city, int month)
    {
        const string sql = @"
SELECT
    City,
    Month,
    AverageHigh,
    AverageLow,
    StandardDeviation
FROM WeatherData
WHERE City = @city AND Month = @month";

        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        // Using the RandomSkunk.Results.Dapper package, query the database for the monthly temperature information
        // for the given city and month. Any errors from making the query are automatically captured in the result.
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

        // Using the RandomSkunk.Results.Dapper package, query the database for the monthly temperature
        // information. Any errors from making the query are automatically captured in the result.
        var monthlyTemperaturesResult = await connection.TryQueryAsync<MonthlyTemperature>(sql);

        // Convert the Result<IEnumerable<MonthlyTemperator>> into a Result<IReadOnlyList<WeatherProfile>>
        // using the Select method. This works very similar to the Select extension method from LINQ.
        return monthlyTemperaturesResult.Select(CreateWeatherProfiles);
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

    private static IReadOnlyList<WeatherProfile> CreateWeatherProfiles(IEnumerable<MonthlyTemperature> monthlyTemperatures)
    {
        return monthlyTemperatures.GroupBy(monthlyTemperature => monthlyTemperature.City)
            .Select(monthlyTemperaturesByCity =>
                new WeatherProfile
                {
                    City = monthlyTemperaturesByCity.Key,
                    MonthlyTemperatures = monthlyTemperaturesByCity.OrderBy(x => x.Month).ToList(),
                })
            .ToList();
    }

    private async Task<Result> UpsertWeatherProfile(WeatherProfile weatherProfile, string sql)
    {
        using DbConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(default);

        using DbTransaction transaction = await connection.BeginTransactionAsync();

        foreach (MonthlyTemperature? monthlyTemperature in weatherProfile.MonthlyTemperatures)
        {
            var param = new
            {
                weatherProfile.City,
                monthlyTemperature.Month,
                monthlyTemperature.AverageHigh,
                monthlyTemperature.AverageLow,
                monthlyTemperature.StandardDeviation,
            };

            // Using the RandomSkunk.Results.Dapper package, execute the query.
            // Any errors from making the query are automatically captured in the result.
            Result<int> rowsAffectedResult = await connection.TryExecuteAsync(sql, param, transaction);

            // The caller doesn't care about the number of affected rows, so just make sure
            // exactly one row was affected.
            Result result = rowsAffectedResult.EnsureOneRowAffected();

            if (result.IsFail)
            {
                // If the query failed or exactly one row was not affected, roll back the
                // transaction and return the Fail result.
                await transaction.RollbackAsync();
                return result;
            }
        }

        // If all queries were successful, commit the transaction and return a Success result.
        await transaction.CommitAsync();
        return Result.Success();
    }
}
