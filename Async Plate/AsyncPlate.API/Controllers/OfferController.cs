using AsyncPlate.API.Models;
using AsyncPlate.Core.DTOs.Category;
using AsyncPlate.Core.DTOs.Offer;
using AsyncPlate.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
{

    [ApiController]
    [Route("api/[controller]s")]
    //[Authorize(Roles = "Admin")]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOffer([FromBody] AddOfferRequestDTO offerRequestDTO)
        {
            var responseDto = await _offerService.AddOfferAsync(offerRequestDTO);
            return Created($"/offers/{responseDto.Id}", new ApiResponse<OfferResponseDTO>(true, "Offer created successfully", responseDto));

        }
    }
}
