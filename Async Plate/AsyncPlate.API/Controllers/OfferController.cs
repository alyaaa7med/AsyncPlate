using AsyncPlate.API.Models;
using AsyncPlate.Application.DTOs.Offer;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Application.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AsyncPlate.Application.Common.DTOs;

namespace AsyncPlate.API.Controllers
{

    [ApiController]
    [Route("api/[controller]s")]
    [Authorize(Roles = "Admin")]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> AddOffer([FromBody] AddOfferRequestDTO offerRequestDTO)
        {
            var responseDto = await _offerService.AddOfferAsync(offerRequestDTO);
            return Created($"/offers/{responseDto.Id}", new ApiResponse<OfferResponseDTO>(true, "Offer created successfully", responseDto));

        }

        [HttpGet("{offerId}")]
        public async Task<IActionResult> GetOfferById([FromRoute] string offerId)
        {
            var responseDto = await _offerService.GetOfferByIdAsync(offerId);
            return Ok(new ApiResponse<OfferResponseDTO>(true, "Offer retrieved successfully", responseDto));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOffers([FromQuery] OfferFilterDTO offerFilter)
        {
            var responseDto = await _offerService.GetAllOffersAsync(offerFilter);
            return Ok(new ApiResponse<PagedResult<OfferResponseDTO>>(true, "Offers retrieved successfully", responseDto));
        }

        [HttpPatch("{offerId}/inactivate")]
        public async Task<IActionResult> InactivateOffer([FromRoute] string offerId)
        {
            var responseDto = await _offerService.InactivateOfferAsync(offerId);
            return Ok(new ApiResponse<OfferResponseDTO>(true, "Offer inactivated successfully", responseDto));
        }

    }
}
