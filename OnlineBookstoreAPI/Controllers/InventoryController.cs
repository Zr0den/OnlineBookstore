using Microsoft.AspNetCore.Mvc;
using OnlineBookstoreCore.Interfaces;
using System.Runtime.CompilerServices;

namespace OnlineBookstoreAPI.Controllers
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

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetStockLevel(string isbn)
        {
            var stock = await _inventoryService.GetStockLevelAsync(isbn);
            if (stock == null) return NotFound();
            return Ok(new { ISBN = isbn, StockLevel = stock });
        }

        [HttpPost("{isbn}/update")]
        public async Task<IActionResult> UpdateStockLevel(string isbn, [FromBody] int quantityChange)
        {
            var success = await _inventoryService.UpdateStockAsync(isbn, quantityChange);
            if (!success) return NotFound(new { Message = "Book not found in inventory" });

            return Ok(new { ISBN = isbn, QuantityChange = quantityChange });
        }

        [HttpPost("{isbn}/set")]
        public async Task<IActionResult> SetStockLevel(string isbn, [FromBody] int quantity)
        {
            await _inventoryService.SetStockLevelAsync(isbn, quantity);
            return Ok(new { ISBN = isbn, UpdatedStockLevel = quantity });
        }
    }
}
