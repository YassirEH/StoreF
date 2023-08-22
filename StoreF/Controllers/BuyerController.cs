﻿using AutoMapper;
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
        private readonly IBuyerRep _buyerRep;

        public BuyerController(IBuyerRep buyerRep, IMapper mapper, INotificationService notificationService)
            : base(mapper, notificationService)
        {
            _buyerRep = buyerRep;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BuyerDto>))]
        public IActionResult GetBuyer()
        {
            var buyers = _buyerRep.GetBuyers();
            var buyerDto = _mapper.Map<List<BuyerDto>>(buyers);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Response(buyerDto);
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
            return Response(buyer);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(BuyerDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateBuyer([FromBody] BuyerDto buyerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var buyer = _mapper.Map<Buyer>(buyerDto);

            _buyerRep.CreateBuyer(buyer);

            _notificationService.Notify("A new buyer has been created", "Success", ErrorType.Success);

            // Assuming buyer.Id contains the generated ID after creation
            return CreatedAtAction(nameof(GetBuyerById), new { buyerId = buyer.Id }, buyerDto);
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
    }
}
