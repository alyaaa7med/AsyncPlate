using AsyncPlate.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Interfaces
{
    public interface IUnitOfWork
    {

        /*
                مش مسؤل عن التعامل مع الانتيتي نفسه لكن بيجمع و يحفظ ف ترانزاكشن واحدة
        */

        ICustomerRepo customers { get; }
        IKitchenChefRepo kitchenChefs { get; }
        IRefreshTokenRepo refreshtokens { get; }
        IOneTimeTokenRepo onetimetokens { get; }

        Task<int> SaveChangesAsync();//int : num of rows affected 

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollBackTransactionAsync();

    }
}
