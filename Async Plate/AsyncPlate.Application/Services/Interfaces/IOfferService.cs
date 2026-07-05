using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Offer;
//using AsyncPlate.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface IOfferService
    {
        Task<OfferResponseDTO> AddOfferAsync(AddOfferRequestDTO offerRequestDTO);
        Task<OfferResponseDTO> GetOfferByIdAsync(string id);
        Task<PagedResult<OfferResponseDTO>> GetAllOffersAsync(OfferFilterDTO offerFilter);
        Task<OfferResponseDTO> InactivateOfferAsync(string offerId);


    }
}
