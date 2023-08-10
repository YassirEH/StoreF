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

        public BuyerController(IBuyerRep buyerRep, IMapper mapper)
        {
            _buyerRep = buyerRep;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BuyerDto>))]
        public IActionResult GetBuyer()
        {
            var buyers = _buyerRep.GetBuyers();
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
            if (!_buyerRep.BuyerExists(buyerId))
                return NotFound();

            var buyer = _mapper.Map<BuyerDto>(_buyerRep.GetBuyer(buyerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(buyer);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]

        public IActionResult CreateBuyer([FromBody] BuyerDto buyerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var buyer = _mapper.Map<Buyer>(buyerDto);

            _buyerRep.CreateBuyer(buyer);
            var createdBuyerDto = _mapper.Map<BuyerDto>(buyer);

            return CreatedAtAction(nameof(GetBuyer), new { buyerId = createdBuyerDto });
        }
        
        [HttpPut("{buyerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateBuyer(int buyerId, BuyerDto buyerDto)
        {
            if (!_buyerRep.BuyerExists(buyerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingBuyer = _buyerRep.GetBuyer(buyerId);
            _mapper.Map(buyerDto, existingBuyer);
            _buyerRep.UpdateBuyer(existingBuyer);
            return NoContent();
        }
        [HttpDelete("{buyerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBuyer(int buyerId)
        {
            if (!_buyerRep.BuyerExists(buyerId))
            {
                return NotFound();
            }

            var buyerToDelete = _buyerRep.GetBuyer(buyerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_buyerRep.DeleteBuyer(buyerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting this buyer");
            }

            return NoContent();
        }
    }
}
