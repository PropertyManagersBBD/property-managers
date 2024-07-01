using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics;

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
		///		size:int,
		///		toRent: bool
		/// }
		/// 
		/// </remarks>
		/// <response code="200"> Good </response>
		/// <response code="400"> Bad</response>
		[HttpPut("Property", Name = "RequestProperty")]
		public IActionResult GetProperties([FromBody] RequestProperty requestProperty)
		{
			try
			{
				if(requestProperty.size > 0 && requestProperty.size <= 8)
				{
					decimal price = _propertyManagerService.GetPrice(requestProperty.size);

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

        /// <summary>
		/// Used to get all properties
		/// </summary>
		/// <returns>Returns all properties</returns>
		/// <remarks>
		/// 
		/// Takes in a page number and page size with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the properties.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("Properties", Name ="GetAllProperties")]
		public IActionResult GetAllProperties(int PageNumber, int PageSize)
		{
            try
            {
                List<Property> properties = _propertyManagerService.GetAllProperties(PageNumber, PageSize);
                return Ok(properties);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all properties filtered by ID
		/// </summary>
		/// <returns>Returns all properties filtered by ID</returns>
		/// <remarks>
		/// 
		/// Takes in an Id, a page number and page size with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the properties.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("Properties/{Id}", Name ="GetByPropertiesId")]
		public IActionResult GetByPropertiesId(long Id, int PageNumber, int PageSize)
		{
            try
            {
                List<Property> properties = _propertyManagerService.GetByPropertiesId(Id, PageNumber, PageSize);
                return Ok(properties);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all properties filtered by Owner ID
		/// </summary>
		/// <returns>Returns all properties filtered by Owner ID</returns>
		/// <remarks>
		/// 
		/// Takes in an Owner Id, a page number and page size with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the properties.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("Properties/Owner/{OwnerId}", Name ="GetByPropertiesOwnerId")]
		public IActionResult GetByPropertiesOwnerId(long OwnerId, int PageNumber, int PageSize)
		{
            try
            {
                List<Property> properties = _propertyManagerService.GetByPropertiesOwnerId(OwnerId, PageNumber, PageSize);
                return Ok(properties);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all properties filtered by Capacity
		/// </summary>
		/// <returns>Returns all properties filtered by Capacity</returns>
		/// <remarks>
		/// 
		/// Takes in an Capacity, a page number and page size with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the properties.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("Properties/Capacity/{Capacity}", Name ="GetByPropertiesCapacity")]
		public IActionResult GetByPropertiesCapacity(int Capacity, int PageNumber, int PageSize)
		{
            try
            {
                List<Property> properties = _propertyManagerService.GetByPropertiesCapacity(Capacity, PageNumber, PageSize);
                return Ok(properties);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all properties filtered by ID and Owner Id
		/// </summary>
		/// <returns>Returns all properties filtered by ID and Owner Id</returns>
		/// <remarks>
		/// 
		/// Takes in an Id, Owner Id, a page number and page size with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the properties.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("Properties/{Id}/Owner/{OwnerId}", Name ="GetByPropertiesIdAndOwnerId")]
		public IActionResult GetByPropertiesIdAndOwnerId(long Id, long OwnerId, int PageNumber, int PageSize)
		{
            try
            {
                List<Property> properties = _propertyManagerService.GetByPropertiesIdAndOwnerId(Id, OwnerId, PageNumber, PageSize);
                return Ok(properties);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all properties filtered by ID and Capacity
		/// </summary>
		/// <returns>Returns all properties filtered by ID and Capacity</returns>
		/// <remarks>
		/// 
		/// Takes in an Id, capacity, a page number and page size with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the properties.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("Properties/{Id}/Capacity/{Capacity}", Name ="GetByPropertiesIdAndCapacity")]
		public IActionResult GetByPropertiesIdAndCapacity(long Id, int Capacity, int PageNumber, int PageSize)
		{
            try
            {
                List<Property> properties = _propertyManagerService.GetByPropertiesIdAndCapacity(Id, Capacity, PageNumber, PageSize);
                return Ok(properties);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all properties filtered by Owner ID and Capacity
		/// </summary>
		/// <returns>Returns all properties filtered by Owner ID and Capacity</returns>
		/// <remarks>
		/// 
		/// Takes in an Owner Id, a capacity, a page number and page size with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the properties.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("Properties/Owner/{OwnerId}/Capacity/{Capacity}", Name ="GetByPropertiesOwnerIdAndCapacity")]
		public IActionResult GetByPropertiesOwnerIdAndCapacity(long OwnerId, int Capacity, int PageNumber, int PageSize)
		{
            try
            {
                List<Property> properties = _propertyManagerService.GetByPropertiesOwnerIdAndCapacity(OwnerId, Capacity, PageNumber, PageSize);
                return Ok(properties);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all properties filtered by ID, Owner Id, and Capacity
		/// </summary>
		/// <returns>Returns all properties filtered by ID, Owner Id, and Capacity</returns>
		/// <remarks>
		/// 
		/// Takes in an ID, Owner Id, and Capacity, a page number and page size with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the properties.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("Properties/{Id}/Owner/{OwnerId}/Capacity/{Capacity}", Name ="GetByPropertiesAllFilter")]
		public IActionResult GetByPropertiesAllFilter(long Id, long OwnerId, int Capacity, int PageNumber, int PageSize)
		{
            try
            {
                List<Property> properties = _propertyManagerService.GetByPropertiesAllFilter(Id, OwnerId, Capacity, PageNumber, PageSize);
                return Ok(properties);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

        /// <summary>
		/// Used to get all sale contracts
		/// </summary>
		/// <returns>Returns all sale contracts</returns>
		/// <remarks>
		/// 
		/// Takes in a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the sale contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("SaleContracts", Name ="GetAllSaleContracts")]
		public IActionResult GetAllSaleContracts(int PageNumber, int PageSize)
		{
            try
            {
                List<SaleContract> saleContracts = _propertyManagerService.GetAllSaleContracts(PageNumber, PageSize);
                return Ok(saleContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all sale contracts by Id
		/// </summary>
		/// <returns>Returns all sale contracts by Id</returns>
		/// <remarks>
		/// 
		/// Takes in an Id, a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the sale contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("SaleContracts/{Id}", Name ="GetBySaleContractsId")]
		public IActionResult GetBySaleContractsId(long Id, int PageNumber, int PageSize)
		{
            try
            {
                List<SaleContract> saleContracts = _propertyManagerService.GetBySaleContractsId(Id, PageNumber, PageSize);
                return Ok(saleContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all sale contracts by Property Id
		/// </summary>
		/// <returns>Returns all sale contracts by Property Id</returns>
		/// <remarks>
		/// 
		/// Takes in a Property Id, a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the sale contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("SaleContracts/Property/{PropertyId}", Name ="GetBySaleContractsPropertyId")]
		public IActionResult GetBySaleContractsPropertyId(long PropertyId, int PageNumber, int PageSize)
		{
            try
            {
                List<SaleContract> saleContracts = _propertyManagerService.GetBySaleContractsPropertyId(PropertyId, PageNumber, PageSize);
                return Ok(saleContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all sale contracts by Capacity
		/// </summary>
		/// <returns>Returns all sale contracts by Capacity</returns>
		/// <remarks>
		/// 
		/// Takes in an Capacity, a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the sale contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("SaleContracts/Capacity/{Capacity}", Name ="GetBySaleContractsCapacity")]
		public IActionResult GetBySaleContractsCapacity(int Capacity, int PageNumber, int PageSize)
		{
            try
            {
                List<SaleContract> saleContracts = _propertyManagerService.GetBySaleContractsCapacity(Capacity, PageNumber, PageSize);
                return Ok(saleContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all sale contracts by Id and PropertyId
		/// </summary>
		/// <returns>Returns all sale contracts by Id and PropertyId</returns>
		/// <remarks>
		/// 
		/// Takes in an Id, a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the sale contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("SaleContracts/{Id}/Property/{PropertyId}", Name ="GetBySaleContractsIdAndPropertyId")]
		public IActionResult GetBySaleContractsIdAndPropertyId(long Id, long PropertyId, int PageNumber, int PageSize)
		{
            try
            {
                List<SaleContract> saleContracts = _propertyManagerService.GetBySaleContractsIdAndPropertyId(Id, PropertyId, PageNumber, PageSize);
                return Ok(saleContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all sale contracts by Id and Capacity
		/// </summary>
		/// <returns>Returns all sale contracts by Id and Capacity</returns>
		/// <remarks>
		/// 
		/// Takes in an Id, capacity, a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the sale contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("SaleContracts/{Id}/Capacity/{Capacity}", Name ="GetBySaleContractsIdAndCapacity")]
		public IActionResult GetBySaleContractsIdAndCapacity(long Id, int Capacity, int PageNumber, int PageSize)
		{
            try
            {
                List<SaleContract> saleContracts = _propertyManagerService.GetBySaleContractsIdAndCapacity(Id, Capacity, PageNumber, PageSize);
                return Ok(saleContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all sale contracts by Id
		/// </summary>
		/// <returns>Returns all sale contracts by Id</returns>
		/// <remarks>
		/// 
		/// Takes in an Id, a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the sale contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("SaleContracts/{Id}/Properties/{PropertyId}/Capacity/{Capacity}", Name ="GetBySaleContractsAllFilter")]
		public IActionResult GetBySaleContractsAllFilter(long Id, long PropertyId, int Capacity, int PageNumber, int PageSize)
		{
            try
            {
                List<SaleContract> saleContracts = _propertyManagerService.GetBySaleContractsAllFilter(Id, PropertyId, Capacity, PageNumber, PageSize);
                return Ok(saleContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

        /// <summary>
		/// Used to get all rental contracts
		/// </summary>
		/// <returns>Returns all rental contracts</returns>
		/// <remarks>
		/// 
		/// Takes in a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the rental contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("RentalContracts", Name ="GetAllRentalContracts")]
		public IActionResult GetAllRentalContracts(int PageNumber, int PageSize)
		{
            try
            {
                List<RentalContract> rentalContracts = _propertyManagerService.GetAllRentalContracts(PageNumber, PageSize);
                return Ok(rentalContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all rental contracts by Id
		/// </summary>
		/// <returns>Returns all rental contracts by Id</returns>
		/// <remarks>
		/// 
		/// Takes in an Id, a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the rental contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("RentalContracts/{Id}", Name ="GetByRentalContractsId")]
		public IActionResult GetByRentalContractsId(long Id, int PageNumber, int PageSize)
		{
            try
            {
                List<RentalContract> rentalContracts = _propertyManagerService.GetByRentalContractsId(Id, PageNumber, PageSize);
                return Ok(rentalContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all rental contracts by Property Id
		/// </summary>
		/// <returns>Returns all rental contracts by Property Id</returns>
		/// <remarks>
		/// 
		/// Takes in a Property Id, a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the rental contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("RentalContracts/Property/{PropertyId}", Name ="GetByRentalContractsPropertyId")]
		public IActionResult GetByRentalContractsPropertyId(long PropertyId, int PageNumber, int PageSize)
		{
            try
            {
                List<RentalContract> rentalContracts = _propertyManagerService.GetByRentalContractsPropertyId(PropertyId, PageNumber, PageSize);
                return Ok(rentalContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all rental contracts by Capacity
		/// </summary>
		/// <returns>Returns all rental contracts by Capacity</returns>
		/// <remarks>
		/// 
		/// Takes in an Capacity, a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the rental contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("RentalContracts/Capacity/{Capacity}", Name ="GetByRentalContractsCapacity")]
		public IActionResult GetByRentalContractsCapacity(int Capacity, int PageNumber, int PageSize)
		{
            try
            {
                List<RentalContract> rentalContracts = _propertyManagerService.GetByRentalContractsCapacity(Capacity, PageNumber, PageSize);
                return Ok(rentalContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all rental contracts by Id and PropertyId
		/// </summary>
		/// <returns>Returns all rental contracts by Id and PropertyId</returns>
		/// <remarks>
		/// 
		/// Takes in an Id, a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the rental contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("RentalContracts/{Id}/Property/{PropertyId}", Name ="GetByRentalContractsIdAndPropertyId")]
		public IActionResult GetByRentalContractsIdAndPropertyId(long Id, long PropertyId, int PageNumber, int PageSize)
		{
            try
            {
                List<RentalContract> rentalContracts = _propertyManagerService.GetByRentalContractsIdAndPropertyId(Id, PropertyId, PageNumber, PageSize);
                return Ok(rentalContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all rental contracts by Id and Capacity
		/// </summary>
		/// <returns>Returns all rental contracts by Id and Capacity</returns>
		/// <remarks>
		/// 
		/// Takes in an Id, capacity, a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the rental contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("RentalContracts/{Id}/Capacity/{Capacity}", Name ="GetByRentalContractsIdAndCapacity")]
		public IActionResult GetByRentalContractsIdAndCapacity(long Id, int Capacity, int PageNumber, int PageSize)
		{
            try
            {
                List<RentalContract> rentalContracts = _propertyManagerService.GetByRentalContractsIdAndCapacity(Id, Capacity, PageNumber, PageSize);
                return Ok(rentalContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}

		/// <summary>
		/// Used to get all rental contracts by Id
		/// </summary>
		/// <returns>Returns all rental contracts by Id</returns>
		/// <remarks>
		/// 
		/// Takes in an Id, a page number with pages starting from 1 to provide a manageable amount of data at a time
		///
		/// </remarks>
		/// <response code="200">
		/// Will return a list of all the rental contracts.
		/// </response>
		/// <response code="400"> 
		/// Will return the error
		/// </response>
		[HttpGet("RentalContracts/{Id}/Properties/{PropertyId}/Capacity/{Capacity}", Name ="GetByRentalContractsAllFilter")]
		public IActionResult GetByRentalContractsAllFilter(long Id, long PropertyId, int Capacity, int PageNumber, int PageSize)
		{
            try
            {
                List<RentalContract> rentalContracts = _propertyManagerService.GetByRentalContractsAllFilter(Id, PropertyId, Capacity, PageNumber, PageSize);
                return Ok(rentalContracts);
            } catch(Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
		}
	}
}
