using Backend.DTOs;

namespace Backend.Services
{
	public interface IWeatherForecastService
	{
		IEnumerable<WeatherForecast> GetWeatherForecasts();
	}
}