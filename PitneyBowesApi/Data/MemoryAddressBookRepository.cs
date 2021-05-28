using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PitneyBowesApi.Models;

namespace PitneyBowesApi.Data
{
    public class MemoryAddressBookRepository : IAddressBookRepository
    {
        private readonly MemoryDbContext _dbContext;
        

        public MemoryAddressBookRepository(MemoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsyc()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Address> GetById(Guid guid)
        {
            return await _dbContext.Addresses.FirstAsync(x => x.Guid == guid);
        }

        public async Task<IQueryable<Address>> GetAll()
        {
            return _dbContext.Addresses.AsQueryable();
        }

        public async Task<IQueryable<Address>> Find(Expression<Func<Address, bool>> expression)
        {
            return _dbContext.Addresses.Where(expression.Compile()).AsQueryable();
        }

        public async Task Add(Address entity)
        {
            await _dbContext.Addresses.AddAsync(entity);
        }

        public async Task Remove(Address entity)
        {
            _dbContext.Addresses.Remove(entity);
        }
    }
}