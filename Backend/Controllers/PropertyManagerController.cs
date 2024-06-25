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
	}
}
