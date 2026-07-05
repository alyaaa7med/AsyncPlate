using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Repositories
{
    public interface IOfferRepo:IBaseRepo<Offer>
    {
        IQueryable<Offer> FilterActive(IQueryable<Offer> query);

        IQueryable<Offer> FilterByCategory(IQueryable<Offer> query, string categoryName);
        IQueryable<Offer> FilterByPercentage(IQueryable<Offer> query, decimal percentage);
        IQueryable<Offer> GetAllOffers();
        Task<Offer?> GetOfferWithCategoryAsync(string offerId);
       

    }
}
