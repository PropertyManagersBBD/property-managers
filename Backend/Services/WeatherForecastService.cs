using Backend.DTOs;

namespace Backend.Services
{
	public class WeatherForecastService : IWeatherForecastService
	{
		public IEnumerable<WeatherForecast> GetWeatherForecasts()
		{
			string[] Summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length+1)] // To stop the controller from sending a bad request sometimes, remove the +1 from here
			}).ToArray();
		}
	}
}
