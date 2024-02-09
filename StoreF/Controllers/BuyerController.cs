using AutoMapper;
using Core.Dto;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using webApi.Application.Services;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuyerController : APIController
    {
        private readonly IMapper _mapper;
        private readonly IBuyerRep _buyerRep;
        private readonly ILogger _logger;

        public BuyerController(IBuyerRep buyerRep, IMapper mapper, INotificationService notificationService, ILogger logger)
            : base(notificationService)
        {
            _mapper = mapper;
            _buyerRep = buyerRep;
            _logger = logger;
        }

        [HttpGet("Get All Buyers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BuyerDto>))]
        public IActionResult GetBuyer()
        {
            try
            {
                var buyers = _buyerRep.GetBuyers();
                var buyerDto = _mapper.Map<List<BuyerDto>>(buyers);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Response(buyerDto);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "A null argument was passed.");
                return BadRequest("A null argument was passed. Please check your request and try again.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation was attempted.");
                return BadRequest("An invalid operation was attempted. Please check your request and try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpGet("{buyerId}")]
        [ProducesResponseType(200, Type = typeof(BuyerDto))]
        [ProducesResponseType(400)]
        public IActionResult GetBuyerById(int buyerId)
        {
            try
            {
                if (!_buyerRep.BuyerExists(buyerId))
                    return NotFound();

                var buyer = _mapper.Map<BuyerDto>(_buyerRep.GetBuyerById(buyerId));

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Response(buyer);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "A null argument was passed.");
                return BadRequest("A null argument was passed. Please check your request and try again.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation was attempted.");
                return BadRequest("An invalid operation was attempted. Please check your request and try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpGet("Filter By Name")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BuyerDto>))]
        [ProducesResponseType(400)]
        public IActionResult FilterByName()
        {
            try
            {
                var buyers = _buyerRep.FilterByName();
                var buyerDto = _mapper.Map<List<BuyerDto>>(buyers);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Response(buyerDto);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "A null argument was passed.");
                return BadRequest("A null argument was passed. Please check your request and try again.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation was attempted.");
                return BadRequest("An invalid operation was attempted. Please check your request and try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }


        [HttpPost("Create Buyer")]
        [ProducesResponseType(201, Type = typeof(BuyerDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateBuyer([FromBody] BuyerDto buyerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var buyer = _mapper.Map<Buyer>(buyerDto);

                _buyerRep.CreateBuyer(buyer);

                _notificationService.Notify("A new buyer has been created", "Success", ErrorType.Success);

                // Assuming buyer.Id contains the generated ID after creation
                return CreatedAtAction(nameof(GetBuyerById), new { buyerId = buyer.Id }, buyerDto);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "A null argument was passed.");
                return BadRequest("A null argument was passed. Please check your request and try again.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation was attempted.");
                return BadRequest("An invalid operation was attempted. Please check your request and try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpPut("Update Buyer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateBuyer(int buyerId, [FromBody] BuyerDto buyerDto)
        {
            try
            {
                if (!_buyerRep.BuyerExists(buyerId))
                    return NotFound();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingBuyer = _buyerRep.GetBuyerById(buyerId);

                existingBuyer.FName = buyerDto.FName;
                existingBuyer.LName = buyerDto.LName;
                existingBuyer.Email = buyerDto.Email;

                _buyerRep.UpdateBuyer(existingBuyer);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "A null argument was passed.");
                return BadRequest("A null argument was passed. Please check your request and try again.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation was attempted.");
                return BadRequest("An invalid operation was attempted. Please check your request and try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpDelete("Delete Buyer/{buyerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBuyer(int buyerId)
        {
            try
            {
                if (!_buyerRep.BuyerExists(buyerId))
                {
                    return NotFound();
                }

                var buyerToDelete = _buyerRep.GetBuyerById(buyerId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool deleteResult = _buyerRep.DeleteBuyer(buyerToDelete);

                if (deleteResult)
                {
                    _notificationService.Notify("The buyer has been deleted", "Success", ErrorType.Success);
                    return NoContent();
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong deleting this buyer");
                    _notificationService.Notify("Error deleting the buyer", "Error", ErrorType.Error);
                    return BadRequest(ModelState);
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "A null argument was passed.");
                return BadRequest("A null argument was passed. Please check your request and try again.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation was attempted.");
                return BadRequest("An invalid operation was attempted. Please check your request and try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }
    }
}