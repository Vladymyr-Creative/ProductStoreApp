using Microsoft.EntityFrameworkCore;
using ProductStoreApp.Interfaces;
using ProductStoreApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductStoreApp.Repository
{
    public class CurrencyRepository : IRepository<Currency>
    {
        private readonly AppDBContext _context;

        public CurrencyRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await _context.Currency.ToListAsync();
        }

        public async Task<Currency> FindByIdAsync(int id)
        {
            return await _context.Currency.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> AddAsync(Currency product)
        {
            await _context.Currency.AddAsync(product);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Currency product)
        {
            _context.Currency.Update(product);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Currency product)
        {
            _context.Currency.Remove(product);
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            // return true if 1 or more entities were changed
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
