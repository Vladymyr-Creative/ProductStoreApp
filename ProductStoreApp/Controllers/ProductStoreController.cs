using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductStoreApp;
using ProductStoreApp.Interfaces;
using ProductStoreApp.Models;
using ProductStoreApp.ViewModels;

namespace ProductStoreApp.Controllers
{
    [Route("[controller]/[action]")]
    public class ProductStoreController : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly IRepository<ProductStore> _context;
        private readonly IRepository<Product> _contextProduct;
        private readonly IRepository<Store> _contextStore;

        public ProductStoreController(
            AppDBContext dbContext,
            IRepository<ProductStore> context,
            IRepository<Product> contextProduct,
            IRepository<Store> contextStore
            )
        {
            _dbContext = dbContext;
            _contextProduct = contextProduct;
            _contextStore = contextStore;
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            var productList = await _dbContext.ProductStore.Include(p => p.Store).Include(p => p.Product).Where(p => p.StoreId == id).ToListAsync();
            var store = await _contextStore.FindByIdAsync(id);
            if (store == null) {
                return NotFound();
            }
            ViewBag.Store = store;
            ViewData["CurrencyId"] = new SelectList(await _dbContext.Currency.ToListAsync(), "Id", "Name");
            return PartialView(productList);
        }

        
        public async Task<IActionResult> Create(int id)
        {            
            ViewData["ProductId"] = new SelectList(await _contextProduct.GetAllAsync(), "Id", "Name");            
            ProductStore model = new ProductStore { StoreId = id };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,StoreId,Quantity")] ProductStore model)
        {
            if (ModelState.IsValid)
            {
                if (_dbContext.ProductStore.Any(p=>p.StoreId== model.StoreId && p.ProductId == model.ProductId)) {
                    return BadRequest("Product in current  store already exists.");
                }
                if (!await _context.AddAsync(model)) {
                    return BadRequest("Something went wrong. ProductStore is not added.");
                }
                return RedirectToAction(nameof(Index), new { id = model.StoreId});
            }
            ViewData["ProductId"] = new SelectList(await _contextProduct.GetAllAsync(), "Id", "Name");
            return PartialView(model);
        }

        [HttpGet("{StoreId}/{ProductId}")]
        public async Task<IActionResult> Edit(int StoreId, int ProductId)
        {
            var model = await _dbContext.ProductStore.Include(p => p.Product).Include(p => p.Store).FirstOrDefaultAsync(p => p.ProductId == ProductId && p.StoreId == StoreId);
            if (model == null) {
                return NotFound();
            }                        
            return PartialView(model);
        }

        [HttpPost("{StoreId}/{ProductId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int StoreId, int ProductId, [Bind("ProductId,StoreId,Quantity")] ProductStore model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateAsync(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }
                return RedirectToAction(nameof(Index), new { id = StoreId });    
            }
            return PartialView(model);
        }

        [HttpGet("{StoreId}/{ProductId}")]
        public async Task<IActionResult> Delete(int StoreId, int ProductId)
        {
            var model = await _dbContext.ProductStore.Include(p => p.Product).Include(p => p.Store).FirstOrDefaultAsync(p => p.ProductId == ProductId && p.StoreId == StoreId);
             if (model == null)
             {
                 return NotFound();
             }

            return PartialView(model);
        }
        
        [HttpPost("{StoreId}/{ProductId}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int StoreId, int ProductId)
        {
            var model = await _dbContext.ProductStore.FirstOrDefaultAsync(p => p.ProductId == ProductId && p.StoreId == StoreId);
            if (model == null) {
                return NotFound();
            }
            var storeId = model.StoreId;
            if (!await _context.DeleteAsync(model)) {

                return BadRequest("Something went wrong. ProductStore is not deleted.");
            }
            return RedirectToAction(nameof(Index), new { id = storeId });
        }

        private async Task<bool> EntityExists(int id)
        {
            return (await _context.FindByIdAsync(id) != null);
        }

        [HttpPost]
        [ActionName("Count")]
        public async Task<decimal> CountAllMoneyInStore(int id, [Bind("CurrencyId")] ProductStoreCountAllViewModel model)
        {
            decimal total = 0;
            var currency  = await _dbContext.Currency.FirstOrDefaultAsync(p=>p.Id == model.CurrencyId);
            if (currency == null) {
                return total;
            }
            
            var products = await _dbContext.ProductStore.Include(p => p.Product).Include(p => p.Store).Where(p => p.StoreId == id).ToListAsync();
            
            foreach (var item in products) {
                total += item.Product.PriceBase * currency.Rate * item.Quantity;
            }
            return Math.Round(total, 2);
        }
    }

    
}
