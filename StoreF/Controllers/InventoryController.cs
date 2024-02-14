using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using webApi.Application.Services;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : APIController
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger _logger;

        public InventoryController(IInventoryService inventoryService, INotificationService notificationService, ILogger logger)
            : base(notificationService)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        [HttpPost("purchase")]
        public IActionResult PurchaseProduct(int productId, int quantity)
        {
            try
            {
                if (_inventoryService.IsProductInStock(productId, quantity))
                {
                    _inventoryService.AddStock(productId, -quantity);
                    _notificationService.Notify("Purchase successful!", "Success", ErrorType.Success);
                    return Response("Purchase successful!");
                }
                else
                {
                    _notificationService.Notify("Product is out of stock.", "Error", ErrorType.Error);
                    return BadRequest("Product is out of stock.");
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

        [HttpPost("add-stock")]
        public IActionResult AddStock(int productId, int quantity)
        {
            try
            {
                _inventoryService.AddStock(productId, quantity);
                _notificationService.Notify("Stock added successfully!", "Success", ErrorType.Success);
                return Ok();
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

        [HttpPost("deduct-stock")]
        public IActionResult DeductStock(int productId, int quantity)
        {
            try
            {
                _inventoryService.AddStock(productId, -quantity);
                _notificationService.Notify($"Deducted {quantity} units of stock for Product ID: {productId}", "Info", ErrorType.Info);
                return Response($"Deducted {quantity} units of stock for Product ID: {productId}");
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

        [HttpPut("update-stock")]
        public IActionResult UpdateStock(int productId, int newStock)
        {
            try
            {
                int currentStock = _inventoryService.GetCurrentStock(productId);
                int adjustmentQuantity = newStock - currentStock;
                _inventoryService.AddStock(productId, adjustmentQuantity);
                _notificationService.Notify($"Updated stock for Product ID: {productId} to {newStock}", "Info", ErrorType.Info);
                return Response($"Updated stock for Product ID: {productId} to {newStock}");
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
