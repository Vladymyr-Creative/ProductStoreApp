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

namespace ProductStoreApp.Controllers
{
    [Route("[controller]/[action]")]
    public class StoreController : Controller
    {
        private readonly IRepository<Store> _context;

        public StoreController(IRepository<Store> context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return PartialView(await _context.GetAllAsync());
        }

        
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address")] Store model)
        {
            if (ModelState.IsValid) {
                if (!await _context.AddAsync(model)) {
                    return BadRequest("Something went wrong. Store is not added.");
                }
                return RedirectToAction(nameof(Index));
            }
            return PartialView(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.FindByIdAsync((int)id);

            if (model == null)
            {
                return NotFound();
            }
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address")] Store model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateAsync(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EntityExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return PartialView(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.FindByIdAsync((int)id);
            if (model == null)
            {
                return NotFound();
            }

            return PartialView(model);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.FindByIdAsync(id);
            if (!await _context.DeleteAsync(model)) {
                return BadRequest("Something went wrong. Store is not deleted.");
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EntityExists(int id)
        {
            return (await _context.FindByIdAsync(id) != null);
        }
    }
}
