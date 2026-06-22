using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Jobs
{
    public interface IOfferJob
    {
        Task SendnNewOfferNotificationsAsync(string offerId);
    }
}
