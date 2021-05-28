using Microsoft.EntityFrameworkCore;
using PitneyBowesApi.Models;

namespace PitneyBowesApi.Data
{
    public class MemoryDbContext : DbContext
    {
        public MemoryDbContext(DbContextOptions<MemoryDbContext> opt) : base(opt)
        {
            
        }
        
        public DbSet<Address> Addresses { get; set; }
    }
}