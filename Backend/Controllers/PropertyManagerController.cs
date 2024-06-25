using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	/// <summary>
	/// Property manager controller
	/// </summary>
	[ApiController]
	[Route("PropertyManager")]
	public class ProperyManagerController : ControllerBase
	{

		private readonly ILogger<ProperyManagerController> _logger;
		private readonly IPropertyManagerService _propertyManagerService;

		public ProperyManagerController(ILogger<ProperyManagerController> logger, IPropertyManagerService propertyManagerService)
		{
			_logger = logger;
			_propertyManagerService = propertyManagerService;
		}


		[HttpGet("test")]
		public IActionResult GetProps()
		{
			try
			{
				var answer= _propertyManagerService.GetTop5Properties();
				return Ok(answer);
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Used to create 50 new properties
		/// </summary>
		/// <returns>Confirmation on amount created</returns>
		/// <remarks>
		/// 
		/// This is meant for the initial propery spawn
		///
		/// </remarks>
		/// <response code="200"> Good </response>
		/// <response code="400"> Bad </response>
		[HttpPost("spawn", Name = "spawn")]
		public IActionResult SpawnProperties()
		{
			try
			{
				int numCreated = _propertyManagerService.SpawnProperties(50);
				return Ok(numCreated + " properties were created");
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		/// <summary>
		/// Used to create a variable amount of new properties
		/// </summary>
		/// <returns>Confirmation on amount created</returns>
		/// <remarks>
		/// 
		/// This is meant to add new properties later on in the world
		///
		/// </remarks>
		/// <response code="200"> Good </response>
		/// <response code="400"> Bad </response>
		[HttpPost("spawn/{num:int}", Name = "spawn_more")]
		public IActionResult SpawnMoreProperties(int num)
		{
			try
			{
				int numCreated = _propertyManagerService.SpawnProperties(num);
				return Ok(numCreated + " properties were created");
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        /// <summary>
        /// Used to get an available property of a given size
        /// </summary>
        /// <returns>Property ID and price of the requested property</returns>
        /// <remarks>
        /// 
        /// requires the query param size
        ///
        /// </remarks>
        /// <response code="200"> Good: body is json that includes the property ID and the price of the property </response>
        /// <response code="400"> Bad: used if missing a query paramter or when there is an error with the request</response>
        [HttpPost(Name = "Property")]
		public IActionResult GetProperties()
		{
			try
			{
				bool houseSizeExissts = HttpContext.Request.Query.ContainsKey("size");
				if (houseSizeExissts)
				{
					//TODO: add functionality
					//also check that authorization header has a valid api key
				return Ok();
				}
				else
				{
					return BadRequest("Incorrect query params");
				}
			}catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
