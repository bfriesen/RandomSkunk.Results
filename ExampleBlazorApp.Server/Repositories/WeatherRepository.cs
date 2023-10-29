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
        _connectionString = configuration.GetConnectionString("WeatherData") ?? throw new ArgumentException("Connection string 'WeatherData' is missing from the configuration.", nameof(configuration));
    }

    public async Task<Result<MonthlyTemperature>> GetMonthlyTemperature(string city, int month)
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

        using SqliteConnection connection = new(_connectionString);
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

        using SqliteConnection connection = new(_connectionString);
        await connection.OpenAsync();

        // Using the RandomSkunk.Results.Dapper package, query the database for the monthly temperature
        // information. Any errors from making the query are automatically captured in the result.
        Result<IEnumerable<MonthlyTemperature>> monthlyTemperaturesResult =
            await connection.TryQueryAsync<MonthlyTemperature>(sql);

        // Project the Result<IEnumerable<MonthlyTemperator>> into a Result<IReadOnlyList<WeatherProfile>>
        // using the Select method. This works very similar to the Select extension method from LINQ.
        Result<IReadOnlyList<WeatherProfile>> weatherProfilesResult =
            monthlyTemperaturesResult.Select(CreateWeatherProfiles);
        return weatherProfilesResult;
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

        // This method uses LINQ-to-Results in order to safely execute a complex workflow. The main thing to remember
        // about LINQ-to-Results is that they are short-circuiting: if evaluating one clause results in a Fail result,
        // no further clauses are evaluated, and the error from that Fail result will be the error of the overall result.
        Result<int> upsertResult = await (

            // Open the connection. If opening the connection fails, no further clauses will be evaluated.
            from connectionOpened in TryCatch.AsResult(() => connection.OpenAsync(default))

            // Begin a transaction if the connection was opened successfully. If this fails, the whole query fails.
            from transaction in TryCatch.AsResult(async () => await connection.BeginTransactionAsync())

            // Insert/update each monthly temperature record if everything has been successful so far. If updating
            // any of the monthly temperatures fail, the query short-circuits.
            from weatherProfileUpdated in weatherProfile.MonthlyTemperatures.TryForEach(async monthlyTemperature =>
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

                    // The caller doesn't care about the number of affected rows, so return a
                    // result that ensures that exactly one row was affected.
                    var upsertResult = rowsAffectedResult.EnsureOneRowAffected();

                    // If the insert/update failed, roll back the transaction.
                    await upsertResult.OnFail(error => transaction.RollbackAsync());

                    return upsertResult.Truncate();
                })

            // If everything has been successful, commit the transaction.
            from transactionCommitted in TryCatch.AsResult(() => transaction.CommitAsync())

            // We don't need the return value of this query, but LINQ requires it, so arbitrarily return the number
            // two. This means that, when successful, our query will always return a result with a value of two.
            select 2);

        // Truncate the arbitrary value of two from the result since we don't need it.
        return upsertResult.Truncate();
    }
}
