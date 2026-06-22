using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Repositories
{
    public interface IBaseRepo<T> where T : class
    {
        //async :  actually wait for the Database
        Task<T?> GetByIdAsync(string id);

        //sequencial => as i will only change the state of entity in the memory to added, updated, deleted 

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        IQueryable<T> GetAll(); // i will return iquerable then add other query for it 


    }
}
