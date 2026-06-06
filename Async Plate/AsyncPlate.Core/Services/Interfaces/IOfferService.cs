using AsyncPlate.Core.Common.DTOs;
using AsyncPlate.Core.DTOs.Category;
using AsyncPlate.Core.DTOs.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Services.Interfaces
{
    public interface IOfferService
    {
        Task<OfferResponseDTO> AddOfferAsync(AddOfferRequestDTO offerRequestDTO);
        Task<OfferResponseDTO> GetOfferByIdAsync(string id);
        Task<OfferResponseDTO> DeleteOfferByIdAsync(string id);
        Task<PagedResult<OfferResponseDTO>> GetAllOffersAsync(OfferFilterDTO offerFilter);
        Task<OfferResponseDTO> UpdateOfferAsync(UpdateOfferRequestDTO offerRequestDTO);


    }
}
