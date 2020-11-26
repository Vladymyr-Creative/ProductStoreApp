using Microsoft.EntityFrameworkCore;
using ProductStoreApp.Interfaces;
using ProductStoreApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductStoreApp.Repository
{
    public class StoreRepository : IRepository<Store>
    {
        private readonly AppDBContext _context;

        public StoreRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Store>> GetAllAsync()
        {
            return await _context.Store.ToListAsync();
        }

        public async Task<Store> FindByIdAsync(int id)
        {
            return await _context.Store.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> AddAsync(Store entity)
        {
            await _context.Store.AddAsync(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Store entity)
        {
            _context.Store.Update(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Store entity)
        {
            _context.Store.Remove(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            // return true if 1 or more entities were changed
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
