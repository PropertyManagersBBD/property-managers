using Backend.DTOs;
using Backend.Services;
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
        
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
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
        /// <response code="200">Returns the weather forcast for the next few days </response>
        /// <response code="400">If the weather forecast obtained was invalid (see WeatherForecastService.cs)</response>
        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult GetWeatherForecast()
        {
            try
            {
                var result = _weatherForecastService.GetWeatherForecasts();
                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
