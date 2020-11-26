using Microsoft.EntityFrameworkCore;
using ProductStoreApp.Interfaces;
using ProductStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductStoreApp.Repository
{
    public class ProductStoreRepository : IRepository<ProductStore>
    {
        private readonly AppDBContext _context;

        public ProductStoreRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductStore>> GetAllAsync()
        {
            return await _context.ProductStore.Include(p => p.Product).Include(p => p.Store).ToListAsync();
        }

        public async Task<ProductStore> FindByIdAsync(int id)
        {
            return await _context.ProductStore.Include(p => p.Product).Include(p => p.Store).FirstOrDefaultAsync(p => p.StoreId == id);
        }

        public async Task<bool> AddAsync(ProductStore entity)
        {
            await _context.ProductStore.AddAsync(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(ProductStore entity)
        {
            _context.ProductStore.Update(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(ProductStore entity)
        {
            _context.ProductStore.Remove(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            // return true if 1 or more entities were changed
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
