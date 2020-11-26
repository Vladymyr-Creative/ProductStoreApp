using Microsoft.EntityFrameworkCore;
using ProductStoreApp.Interfaces;
using ProductStoreApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductStoreApp.Repository
{    
    public class CategoryRepository : IRepository<Category>
    {
        private readonly AppDBContext _context;

        public CategoryRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            return await _context.Category.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> AddAsync(Category entity)
        {
            await _context.Category.AddAsync(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Category entity)
        {
            _context.Category.Update(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Category entity)
        {
            _context.Category.Remove(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            // return true if 1 or more entities were changed
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
