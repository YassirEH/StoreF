using AutoMapper;
using Core.Application.Dto;
using Core.Application.Interfaces;
using Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuyerController : ControllerBase
    {
        private readonly IBuyerRep _buyerRep;
        private readonly IMapper _mapper;
        private readonly IGenericRep _genericRep;

        public BuyerController(IBuyerRep buyerRep, IMapper mapper, IGenericRep genericRep)
        {
            _buyerRep = buyerRep;
            _mapper = mapper;
            _genericRep = genericRep;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BuyerDto>))]
        public IActionResult GetBuyer()
        {
            var buyers = _genericRep.GetBuyers();
            var buyerDto = _mapper.Map<List<BuyerDto>>(buyers);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(buyerDto);
        }

        [HttpGet("{buyerId}")]
        [ProducesResponseType(200, Type = typeof(BuyerDto))]
        [ProducesResponseType(400)]
        public IActionResult GetBuyerById(int buyerId)
        {
            if (!_genericRep.BuyerExists(buyerId))
                return NotFound();

            var buyer = _mapper.Map<BuyerDto>(_genericRep.GetBuyer(buyerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(buyer);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]

        [HttpPost]
        public IActionResult CreateBuyer([FromBody] BuyerDto buyerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var buyer = _mapper.Map<Buyer>(buyerDto);

            _genericRep.CreateBuyer(buyer);

            // Assuming buyer.Id contains the generated ID after creation
            return CreatedAtAction(nameof(GetBuyerById), new { buyerId = buyer.Id }, buyerDto);
        }

        [HttpPut("{buyerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateBuyer(int buyerId, BuyerDto buyerDto)
        {
            if (!_genericRep.BuyerExists(buyerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingBuyer = _genericRep.GetBuyer(buyerId);
            _mapper.Map(buyerDto, existingBuyer);
            _genericRep.UpdateBuyer(existingBuyer);
            return NoContent();
        }
        [HttpDelete("{buyerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBuyer(int buyerId)
        {
            if (!_genericRep.BuyerExists(buyerId))
            {
                return NotFound();
            }

            var buyerToDelete = _genericRep.GetBuyer(buyerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_genericRep.DeleteBuyer(buyerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting this buyer");
            }

            return NoContent();
        }
    }
}
