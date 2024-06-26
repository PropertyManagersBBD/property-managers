using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
        /// Used to start the simulation, and creaste 5000 random properties
        /// </summary>
        /// <returns>Confirmation on amount created</returns>
        /// <remarks>
        /// 
        /// This is meant for the initial propery spawn
		/// No body is required
		/// Requires an authorization header
        ///
        /// </remarks>
        /// <response code="200"> Good </response>
        /// <response code="400"> Bad </response>
        [HttpPost("initialize", Name = "initialize")]
        public IActionResult SpawnProperties()
        {
            try
            {
                int numCreated = _propertyManagerService.SpawnProperties(5000);
                return Ok(numCreated + " properties were created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




		/// <summary>
		/// Used to set the new price of the property
		/// </summary>
		/// <returns>Nothing</returns>
		/// <remarks>
		/// 
		/// This is meant for the initial propery spawn
		/// One path variable is expected, which is the new price per unit of housing.
		///
		/// </remarks>
		/// <response code="200"> The new price per unit was set </response>
		/// <response code="400"> An error occurred, so the old price per unit was used </response>
		[HttpPut("setPrice/{newPrice}", Name = "Set Price per housing unit")]
		public IActionResult SetPrice(decimal newPrice)
		{
			try
			{
				_propertyManagerService.SetPrice(newPrice);
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
        /// the body is strucutred as follows:
		/// {
		/// size:int
		/// renting:boolean
		/// sellerId:long -> also acts as the land lord ID
		/// buyerId:long -> this also acts as the renter ID
		/// 
		/// }
        ///
        /// </remarks>
        /// <response code="200"> Good: body is json that includes the property ID and the price of the property </response>
        /// <response code="400"> Bad: used if missing a query paramter or when there is an error with the request</response>
        [HttpPost("property",Name = "Request_poperty")]
		public IActionResult GetProperties([FromBody] RequestProperty requestProperty)
		{
			try
			{
				Debug.WriteLine(requestProperty.ToString());
                return(Ok());
			}catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        /// <summary>
        /// Used to get the owner ID of a given property
        /// </summary>
        /// <returns>owner ID of the requested property</returns>
        /// <remarks>
        /// 
        /// requires the poperty ID in the url
        ///
        /// </remarks>
        /// <response code="200"> Good: body is json that includes the property ID and the price of the property </response>
        /// <response code="400"> Bad: used if missing a query paramter or when there is an error with the request</response>
        [HttpGet("Owner/{propertyID}", Name ="Get_Owner")]
		public IActionResult GetOwner()
		{
			return(Ok());	
		}

        /// <summary>
        /// Used to list a property on the market to be sold
        /// </summary>
		/// 
        /// <returns>200 or a 400</returns>
        /// <remarks>
        /// 
        /// Body requres ownerID
        ///
        /// </remarks>
        /// <response code="200"> Good: property is now listed on the market </response>
        /// <response code="400"> Bad:failed to list property</response>
        [HttpPost("sell", Name = "sell_property")]
        public IActionResult sellProperty()
        {
            return (Ok());
        }



        /// <summary>
        /// Used to list a property on the market to be rented
        /// </summary>
        /// 
        /// <returns>200 or a 400</returns>
        /// <remarks>
        /// 
        /// Body requres ownerID
        ///
        /// </remarks>
        /// <response code="200"> Good: property is now rentable </response>
        /// <response code="400"> Bad:failed to list property</response>
        [HttpPost("rent", Name = "rent_property")]
        public IActionResult rentProperty()
        {
            return (Ok());
        }
    }
}
