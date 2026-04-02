using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Interfaces
{
    public interface IBaseRepo<T> where T :class
    {
        //async :  actually wait for the Database
        //get all 
        //get by id 
        //add
        Task<T?> GetByIdAsync(int id); 
        Task<IEnumerable<T>> GetAllAsync() ;
        Task AddAsync(T entity);


        //sequencial => as i will only mark in the memory to updated or deleted 
        //update 
        //delete 
        void Update(T entity);
        void Delete(T entity);

    }
}
