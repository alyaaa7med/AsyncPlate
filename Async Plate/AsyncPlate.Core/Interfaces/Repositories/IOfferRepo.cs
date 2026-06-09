using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Repositories
{
    public interface IOfferRepo:IBaseRepo<Offer>
    {
        Task<Offer?> GetOfferByCategoryAsync(string category);
        IQueryable<Offer> GetOffersByPercentageAsync(decimal percentage);//will be paginated 
        IQueryable<Offer> GetActiveOffers();
    }
}
