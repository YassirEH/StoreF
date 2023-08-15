using Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public PurchaseController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost("purchase")]
        public IActionResult PurchaseProduct(int productId, int quantity)
        {
            // Check if the product is in stock
            if (_inventoryService.IsProductInStock(productId, quantity))
            {
                // Process the purchase logic
                // For example, update the order history and calculate prices

                UpdateOrderHistory(productId, quantity);
                decimal totalPrice = CalculateTotalPrice(productId, quantity);

                // Simulate payment processing (replace with actual payment logic)
                bool paymentSuccessful = SimulatePaymentProcessing(totalPrice);

                if (paymentSuccessful)
                {
                    // Update stock after purchase
                    _inventoryService.UpdateStock(productId, -quantity); // Decrease stock by purchased quantity
                    return Ok("Purchase successful!");
                }
                else
                {
                    return BadRequest("Payment processing failed.");
                }
            }
            else
            {
                return BadRequest("Product is out of stock.");
            }
        }

        [HttpGet("stock")]
        public IActionResult GetProductStock(int productId)
        {
            int currentStock = _inventoryService.GetCurrentStock(productId);
            return Ok(new { ProductId = productId, Stock = currentStock });
        }

        [HttpGet("in-stock")]
        public IActionResult CheckProductInStock(int productId, int requiredQuantity)
        {
            bool isProductInStock = _inventoryService.IsProductInStock(productId, requiredQuantity);
            return Ok(new { ProductId = productId, InStock = isProductInStock });
        }

        [HttpPost("adjust-stock")]
        public IActionResult AdjustProductStock(int productId, int adjustmentQuantity)
        {
            _inventoryService.AdjustStock(productId, adjustmentQuantity);
            return Ok("Stock adjusted successfully.");
        }

        private void UpdateOrderHistory(int productId, int quantity)
        {
            // Simulate order history update (replace with actual logic)
            Console.WriteLine($"Order placed for Product ID: {productId}, Quantity: {quantity}");
        }

        private decimal CalculateTotalPrice(int productId, int quantity)
        {
            // Simulate price calculation (replace with actual logic)
            // For demonstration, assuming a fixed price of $10 per product
            decimal productPrice = 10.0m;
            return productPrice * quantity;
        }

        private bool SimulatePaymentProcessing(decimal totalPrice)
        {
            // Simulate payment processing (replace with actual payment logic)
            // For demonstration, assuming payment is always successful
            return true;
        }
    }
}
