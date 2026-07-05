using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class OfferRepo : GenericRepo<Offer>, IOfferRepo
    {
        public OfferRepo(AppDbContext context) : base(context)
        {

        }

        public IQueryable<Offer> FilterByCategory(IQueryable<Offer> query, string categoryName)
        {
            return query.Where(o => o.Categories.Any(c => c.Name == categoryName));
        }

        public IQueryable<Offer> FilterByPercentage(IQueryable<Offer> query, decimal percentage)
        {
            return query.Where(o => o.DiscountPercentage >= percentage);
        }

        public IQueryable<Offer> FilterActive(IQueryable<Offer> query)
        {
            var currentDate = DateTime.UtcNow;

            return query.Where(o =>
                o.IsActive &&
                o.StartDate <= currentDate &&
                (!o.EndDate.HasValue || o.EndDate >= currentDate));
        }

        public IQueryable<Offer> GetAllOffers()
        {
            return _context.Offers.Include(o => o.Categories).AsQueryable();

        }
        public async Task<Offer?> GetOfferWithCategoryAsync(string offerId)
        {
            return await _context.Offers
                .Include(o => o.Categories)
                .FirstOrDefaultAsync(o => o.Id == offerId);
        }
    }
}