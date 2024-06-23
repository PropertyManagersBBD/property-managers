using Backend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    /// <summary>
    /// Weather forecast controller for retrieving weather forecasts.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of weather forecasts.
        /// </summary>
        /// <returns>An array of weather forecasts.</returns>
        /// <remarks>
        /// Sample response:
        ///
        ///     GET /GetWeatherForecast
        ///     [
        ///		  {
        ///			"date": "2024-06-24",
        ///			"temperatureC": 47,
        ///			"temperatureF": 116,
        ///			"summary": "Sweltering"
        ///		  },
        ///		  {
        ///			"date": "2024-06-25",
        ///			"temperatureC": -11,
        ///			"temperatureF": 13,
        ///			"summary": "Freezing"
        ///		  },
        ///		  {
        ///			"date": "2024-06-26",
        ///			"temperatureC": 30,
        ///			"temperatureF": 85,
        ///			"summary": "Cool"
        ///		  },
        ///		  {
        ///			"date": "2024-06-27",
        ///			"temperatureC": 23,
        ///			"temperatureF": 73,
        ///			"summary": "Balmy"
        ///		  },
        ///		  {
        ///			"date": "2024-06-28",
        ///			"temperatureC": 21,
        ///			"temperatureF": 69,
        ///			"summary": "Mild"
        ///		  }
        ///		]
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
