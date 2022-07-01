# Example Blazor Client App

These are the interesting parts of this app:

- Typed HTTP Clients
    - Uses RandomSkunk.Results.Http to make HTTP requests to the server.
    - WeatherForcastClient
    - WeatherProfileClient
- Razor pages
    - Uses one of the typed HTTP clients to make a request to the server, then handles the result.
    - FiveDayForecast
    - WeatherProfiles
