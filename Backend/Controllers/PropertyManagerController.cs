﻿using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	/// <summary>
	/// Property manager controller
	/// </summary>
	[ApiController]
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
		[HttpPut("SetPrice/{newPrice}", Name = "Set Price per housing unit")]
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
        /// body requires size
		/// 
		/// {
		///		body:int
		/// }
		/// 
        /// </remarks>
        /// <response code="200"> Good </response>
        /// <response code="400"> Bad</response>
        [HttpPut("Property",Name = "RequestProperty")]
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

		/// <summary>
		/// Used to get the owner ID of a given property
		/// </summary>
		/// <returns>owner ID of the requested property</returns>
		/// <remarks>
		/// 
		/// requires the property ID in the url
		///
		/// </remarks>
		/// <response code="200"> Will return the property owner's Id </response>
		/// <response code="400"> 
		/// Will either return:
		/// "Property {propertyId} does not exist" OR
		/// "Property {propertyId} does not have an owner"
		/// </response>
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
        /// Transfers ownership of property or cancels transfer
        /// </summary>
        /// <returns>200 or a 400</returns>
        /// <remarks>
        /// 
        /// Body requires propertyId, sellerId, buyerId, price, approval
        /// 
        /// {
        ///     propertyId:long
        ///     sellerId:long
        ///     buyerId:long
        ///     price:string
        ///     approval:bool
        /// }
        /// 
        /// </remarks>
        /// <response code="200"> Good</response>
        /// <response code="400"> Bad</response>
        [HttpPut("Approval", Name = "Approval")]
        public IActionResult ApproveProperty()
        {
            return (Ok());
        }
    }
}
