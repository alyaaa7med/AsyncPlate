using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Interfaces.Repositories;
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

        public async Task<Offer?> GetOfferByCategoryAsync(string category)
        {
            return await _context.Offers.FirstOrDefaultAsync(o => o.Categories.Any(c => c.Name == category));

        }

        public IQueryable<Offer> GetOffersByPercentageAsync(decimal percentage)
        {
            return _context.Offers.Where(o => o.DiscountPercentage >= percentage);
        }

        public IQueryable<Offer> GetActiveOffers()
        {
            var currentDate = DateTime.UtcNow;
            return _context.Offers.Where(o => o.IsActive && o.StartDate <= currentDate 
                                        && (o.EndDate >= currentDate));
        }


    }
}
