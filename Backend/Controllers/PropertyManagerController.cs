using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics;

namespace Backend.Controllers
{
	/// <summary>
	/// Property manager controller
	/// </summary>
	//[Authorize]
	[ApiController]
	[EnableCors("_myAllowSpecificOrigins")]
	[Route("PropertyManager")]
	public class PropertyManagerController : ControllerBase
	{

		private readonly ILogger<PropertyManagerController> _logger;
		private readonly IPropertyManagerService _propertyManagerService;

		public PropertyManagerController(ILogger<PropertyManagerController> logger, IPropertyManagerService propertyManagerService)
		{
			_logger = logger;
			_propertyManagerService = propertyManagerService;
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
		//[Authorize]
		[HttpPut("SetPrice/{newPrice}", Name = "Set Price per housing unit")]
		public IActionResult SetPrice(long newPrice)
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
		/// body requires size
		/// 
		/// {
		///		size:int,
		///		toRent: bool
		/// }
		/// 
		/// </remarks>
		/// <response code="200"> Good </response>
		/// <response code="400"> Bad</response>
		//[Authorize]
		[HttpPut("Property", Name = "RequestProperty")]
		public IActionResult GetProperties([FromBody] RequestProperty requestProperty)
		{
			try
			{
				if(requestProperty.size > 0 && requestProperty.size <= 8)
				{
					long price = _propertyManagerService.GetPrice(requestProperty.size);

					long propertyId = _propertyManagerService.GetProperty(requestProperty.size, requestProperty.toRent);

					if(propertyId == -1)
					{
						return BadRequest("No Property Is Available");
					}
					else
					{
						var response = new PropertyResponse(price, propertyId);
						return (Ok(response));
					}

				}
				else
				{
					return BadRequest("Invalid Size");
				}
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
		/// requires the property ID in the url
		///
		/// </remarks>
		/// <response code="200">
		/// Will return the property owner's Id.
		/// An Id of -1 indicates that the property is owned by the central revenue service.
		/// </response>
		/// <response code="400"> 
		/// Will return:
		/// "Property {propertyId} does not exist"
		/// </response>
		//[Authorize]
		[HttpGet("Owner/{propertyID}", Name ="GetOwner")]
		public IActionResult GetOwner(long propertyID)
		{
            try
            {
                var owner = _propertyManagerService.GetPropertyOwner(propertyID);
                return Ok(owner);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

        /// <summary>
        /// Used to list a property on the market to be sold
        /// </summary>
		/// 
        /// <returns>200 or a 400</returns>
        /// <remarks>
        /// 
        /// Body requres the property ID
        ///
        /// </remarks>
        /// <response code="200"> Good </response>
        /// <response code="400"> Bad</response>
		//[Authorize]
		[HttpPost("Sell", Name = "SellProperty")]
        public IActionResult SellProperty(int Id)
        {
            try
			{
                _propertyManagerService.ListForSale(Id);
                return Ok("Proprty " + Id + " has been listed for sale");
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// test end point to check if service is alive
		/// </summary>
		/// 
		/// <returns>200 or a 500</returns>
		/// <remarks>
		/// 
		/// 
		///
		/// </remarks>
		/// <response code="200"> Good </response>
		/// <response code="400"> Bad</response>
		//[Authorize]
		[HttpGet("ping", Name="Ping")]
        public IActionResult ping()
        {
            return (Ok("pong"));
        }


		/// <summary>
        /// Used to list a property on the market to be rented
        /// </summary>
        /// 
        /// <returns>200 or a 400</returns>
        /// <remarks>
        /// 
        /// Body requres the property ID
        ///
        /// </remarks>
        /// <response code="200"> Good </response>
        /// <response code="400"> Bad</response>
		//[Authorize]
		[HttpPost("Rent", Name = "RentProperty")]
        public IActionResult RentProperty(int Id)
        {
            try
			{
                _propertyManagerService.ListForRent(Id);
				return Ok("Proprty " + Id + " has been listed for rent");
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
        }

		/// <summary>
		/// ApproveSale
		/// </summary>
		/// <returns>200 or a 400</returns>
		/// <remarks>
		/// 
		/// Body requires:
		/// {
		///     PropertyId:long
		///     BuyerId:long
		///     SellerId:long
		///     Price:long
		///     Approval:bool
		/// }
		/// 
		/// </remarks>
		/// <response code="200"> Good</response>
		/// <response code="400"> Bad</response>
		//[Authorize]
		[HttpPut("ApproveSale", Name = "ApproveSale")]
		public IActionResult ApprovePropertySale([FromBody] SaleApprovalDto saleApprovalDto)
		{
			try
			{
				var ans = _propertyManagerService.ApprovePropertySale(saleApprovalDto);
				return Ok(ans);
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// ApproveRental
		/// </summary>
		/// <returns>200 or a 400</returns>
		/// <remarks>
		/// 
		/// Body requires: 
		/// {
		///     PropertyId: long
		/// 	LandlordId: long
		///     TenantId: long
		/// 	Price: long
		///     IsActive: bool
		/// 	Approval: bool
		/// }
		/// 
		/// </remarks>
		/// <response code="200"> True or false, true for success, false if unsuccess </response>
		/// <response code="400"> Something bad happened </response>
		//[Authorize]
		[HttpPut("ApproveRental", Name = "ApproveRental")]
		public IActionResult ApprovePropertyRental([FromBody] RentalApprovalDto rentalApprovalDto)
		{
			try
			{
				var ans = _propertyManagerService.ApprovePropertyRental(rentalApprovalDto);
				return Ok(ans);
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		/// <summary>
		/// Used to get all properties
		/// </summary>
		/// <returns>Returns properties according to filter</returns>
		/// <remarks>
		/// 
		/// Takes in a page number and page size with pages starting from 1 to provide a manageable amount of data at a time can also take in Id, Owner Id and Capacity
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of properties.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		//[Authorize]
		[HttpGet("Properties", Name ="GetProperties")]
		public IActionResult GetProperties(int PageNumber, int PageSize, long? Id, long? OwnerId, int? Capacity)
		{
            try
            {
                List<Property> properties = _propertyManagerService.GetProperties(PageNumber, PageSize, Id, OwnerId, Capacity);
                return Ok(properties);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

        /// <summary>
		/// Used to get sale contracts
		/// </summary>
		/// <returns>Returns sale contracts according to filter</returns>
		/// <remarks>
		/// 
		/// Takes in a page number with pages starting from 1 to provide a manageable amount of data at a time can also take in Id, Property Id and Capacity
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of sale contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		//[Authorize]
		[HttpGet("SaleContracts", Name ="GetSaleContracts")]
		public IActionResult GetSaleContracts(int PageNumber, int PageSize, long? Id, long? OwnerId, int? Capacity)
		{
            try
            {
                List<SaleContract> saleContracts = _propertyManagerService.GetSaleContracts(PageNumber, PageSize, Id, OwnerId, Capacity);
                return Ok(saleContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get rental contracts
		/// </summary>
		/// <returns>Returns rental contracts according to filter</returns>
		/// <remarks>
		/// 
		/// Takes in a page number with pages starting from 1 to provide a manageable amount of data at a time can also take in Id, Property Id and Capacity
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of rental contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		//[Authorize]
		[HttpGet("RentalContracts", Name = "GetRentalContracts")]
		public IActionResult GetRentalContracts(int PageNumber, int PageSize, long? Id, long? PropertyId, int? Capacity)

		{
			try
			{
				List<RentalContract> rentalContracts = _propertyManagerService.GetRentalContracts(PageNumber, PageSize, Id, PropertyId, Capacity);
				return Ok(rentalContracts);
			}
			catch(Exception ex)
			{
				return BadRequest($"{ex.Message}");
			}
		}

		/// <summary>
		/// Reset
		/// </summary>
		/// <returns>None</returns>
		/// <remarks>
		/// 
		///
		/// </remarks>
		/// <response code="200">
		/// </response>
		/// <response code="400"> 
		/// </response>
		//[Authorize]
		[HttpGet("Reset", Name = "Reset")]
		public IActionResult Reset()

		{
			try
			{
				_propertyManagerService.Reset();
				return Ok();
			}
			catch(Exception ex)
			{
				return BadRequest($"{ex.Message}");
			}
		}

		/// <summary>
		/// Used to get properties belonging to certain owners
		/// </summary>
		/// <returns>Returns properties owned by all the owners in a list</returns>
		/// <remarks>
		/// 
		/// Takes in a list of owner Ids
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of properties.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		//[Authorize]
		[HttpPost("Properties/Owners", Name ="GetPropertiesByOwners")]
		public IActionResult GetPropertiesByOwners(long[] ownerIds)
		{
            try
            {
                List<PropertySummary> properties = _propertyManagerService.GetPropertiesByOwners(ownerIds);
                return Ok(properties);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to transfer ownership to next of kin upon deaths
		/// </summary>
		/// <returns>Returns a confirmation on if it was run correctly</returns>
		/// <remarks>
		/// 
		/// Takes in a list of owner Ids
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a confirmation of having run.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		//[Authorize]
		[HttpPost("dailyUpdate", Name ="DeathTransfer")]
		public IActionResult DeathTransfer(DailyUpdateModel dailyUpdateModel)
		{
            try
            {
				_propertyManagerService.DailyUpdate(dailyUpdateModel.deaths);
                return Ok("Death transfers completed succesfully");
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}
	}
}
