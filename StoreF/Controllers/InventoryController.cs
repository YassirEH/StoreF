using Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost("purchase")]
        public IActionResult PurchaseProduct(int productId, int quantity)
        {
            if (_inventoryService.IsProductInStock(productId, quantity))
            {
                _inventoryService.AddStock(productId, -quantity); 
                return Ok("Purchase successful!");
            }
            else
            {
                return BadRequest("Product is out of stock.");
            }
        }

        [HttpPost("add-stock")]
        public IActionResult AddStock(int productId, int quantity)
        {
            _inventoryService.AddStock(productId, quantity);
            return Ok();
        }

        [HttpPost("deduct-stock")]
        public IActionResult DeductStock(int productId, int quantity)
        {
            _inventoryService.AddStock(productId, -quantity);
            return Ok();
        }

        [HttpPut("update-stock")]
        public IActionResult UpdateStock(int productId, int newStock)
        {
            int currentStock = _inventoryService.GetCurrentStock(productId);
            int adjustmentQuantity = newStock - currentStock;
            _inventoryService.AddStock(productId, adjustmentQuantity);
            return Ok();
        }
    }
}
