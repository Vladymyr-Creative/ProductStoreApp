using Microsoft.EntityFrameworkCore;
using ProductStoreApp.Interfaces;
using ProductStoreApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductStoreApp.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly AppDBContext _context;

        public ProductRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Product.Include(p => p.Category).Include(p => p.Currency).ToListAsync();
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            return await _context.Product.Include(p => p.Category).Include(p => p.Currency).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> AddAsync(Product entity)
        {
            await _context.Product.AddAsync(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Product entity)
        {
            _context.Product.Update(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Product entity)
        {
            _context.Product.Remove(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            // return true if 1 or more entities were changed
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
