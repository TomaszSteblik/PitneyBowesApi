using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PitneyBowesApi.Models;

namespace PitneyBowesApi.Data
{
    public interface IAddressBookRepository
    {
        Task<int> SaveChangesAsyc();
        Task<Address> GetById(Guid guid);
        //using IQueryable to make transition to entity framework easier if needed
        Task<IQueryable<Address>> GetAll();
        Task<IQueryable<Address>> Find(Expression<Func<Address, bool>> expression);
        Task Add(Address entity);
        Task Remove(Address entity);
    }
}