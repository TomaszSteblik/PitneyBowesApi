using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PitneyBowesApi.Models;

namespace PitneyBowesApi.Data
{
    public class MemoryAddressBookRepository : IAddressBookRepository
    {
        private readonly List<Address> _addresses;

        private readonly MemoryDbContext _dbContext;
        

        public MemoryAddressBookRepository(MemoryDbContext dbContext)
        {
            _dbContext = dbContext;
            _addresses = new List<Address>();
        }

        public async Task<int> SaveChangesAsyc()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Address> GetById(Guid guid)
        {
            return _addresses.First(x => x.Guid == guid);
        }

        public async Task<IQueryable<Address>> GetAll()
        {
            return _addresses.AsQueryable();
        }

        public async Task<IQueryable<Address>> Find(Expression<Func<Address, bool>> expression)
        {
            return _addresses.Where(expression.Compile()).AsQueryable();
        }

        public async Task Add(Address entity)
        {
            _addresses.Add(entity);
        }

        public async Task Remove(Address entity)
        {
            _addresses.Remove(entity);
        }
    }
}