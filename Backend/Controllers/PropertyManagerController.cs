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
        [HttpGet(Name = "Property")]
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
