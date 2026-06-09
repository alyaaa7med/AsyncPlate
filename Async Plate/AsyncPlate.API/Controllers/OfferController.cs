using AsyncPlate.API.Models;
using AsyncPlate.Application.DTOs.Offer;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Application.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> AddOffer([FromBody] AddOfferRequestDTO offerRequestDTO)
        {
            var responseDto = await _offerService.AddOfferAsync(offerRequestDTO);
            return Created($"/offers/{responseDto.Id}", new ApiResponse<OfferResponseDTO>(true, "Offer created successfully", responseDto));

        }
    }
}
