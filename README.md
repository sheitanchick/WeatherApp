# WeatherApp
This application is capable to fetch basic forecast info for provided location (latitude and longitude) from two sources.
These are: https://openweathermap.org/ and https://www.weatherbit.io/

To run appplication you supposed to add ApiKeys for both OpenWeather and Weatherbit providers.
Keys can be set in appsettings.json in appropriate section (Weatherbit:ApiKey and OpenWeather:ApiKey) or by adding as an environment variables.

To start application along with database make sure that docker service is running on your machine and execute following commands:
- docker-compose build
- docker-compose up
