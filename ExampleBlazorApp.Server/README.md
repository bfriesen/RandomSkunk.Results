# Example Blazor Server App

These are the interesting parts of this app:

- WeatherRepository
    - Uses RandomSkunk.Results.Dapper to make queries to the database.
    - GetMonthlyTemperature
    - GetWeatherProfiles
    - UpsertWeatherProfile
- WeatherForecastSimulator
    - Uses WeatherRepository to make queries to the database.
- Controllers
    - WeatherForecastController
        - Uses WeatherForecastSimulator to simulate weather forecasts.
    - WeatherProfilesController
        - Uses WeatherRepository to make queries to the database.
        - GetWeatherProfiles
        - AddWeatherProfile
        - EditWeatherProfile
