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
		/// Summary
		/// </summary>
		/// <returns>---------</returns>
		/// <remarks>
		/// 
		/// Remarks go here
		///
		/// </remarks>
		/// <response code="200"> Good </response>
		/// <response code="400"> Bad </response>
		[HttpPut(Name = "spawn")]
		public IActionResult SpawnProperties()
		{
			try
			{
				_propertyManagerService.SpawnProperties();
				return Ok();
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
