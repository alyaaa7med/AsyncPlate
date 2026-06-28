using AsyncPlate.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces
{
    public interface IUnitOfWork
    {

        /*
                مش مسؤل عن التعامل مع الانتيتي نفسه لكن بيجمع و يحفظ ف ترانزاكشن واحدة
        */

        ICustomerRepo customers { get; }
        IKitchenChefRepo kitchenChefs { get; }
        IAdminRepo admins { get; }
        IRefreshTokenRepo refreshtokens { get; }
        IOneTimeTokenRepo onetimetokens { get; }
        ISupplierRepo suppliers { get; }
        IInventoryRepo inventories { get; }
        IRecipeRepo recipes { get; }
        IProductRepo products { get; }
        IProductExtraRepo ProductExtras { get; }
        ICategoryRepo categories { get; }
        IOfferRepo offers { get; }
        IOrderRepo orders { get; }
        INotificationRepo notifications { get; }

        Task<int> SaveChangesAsync();//int : num of rows affected 

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollBackTransactionAsync();

    }
}
