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
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _context;

        public CategoryController(IRepository<Category> context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.GetAllAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category model)
        {
            if (ModelState.IsValid)
            {
                if (!await _context.AddAsync(model)) {
                    return BadRequest("Something went wrong. Category is not added.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
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
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category model)
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
            return View(model);
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

            return View(model);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.FindByIdAsync(id);
            if (!await _context.DeleteAsync(model)) {
                return BadRequest("Something went wrong. Category is not deleted.");
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EntityExists(int id)
        {
            return (await _context.FindByIdAsync(id) != null);
        }
    }
}
