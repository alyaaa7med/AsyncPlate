using AsyncPlate.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
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
    }
}
