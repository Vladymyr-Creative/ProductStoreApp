using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductStoreApp;
using ProductStoreApp.Interfaces;
using ProductStoreApp.Models;
using ProductStoreApp.ViewModels;
using ZXing;
using ZXing.QrCode;

namespace ProductStoreApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBContext _dbContext;       
        private readonly IRepository<Product> _context;
        private readonly IRepository<Category> _contextCategory;
        private readonly IRepository<Currency> _contextCurrency;
        private readonly IMapper _mapper;

        public ProductController(
            AppDBContext dbContext,
            IRepository<Product> context,
            IRepository<Category> contextCategory,
            IRepository<Currency> contextCurrency,            
            IMapper mapper
            )
        {
            _dbContext = dbContext;
            _contextCurrency = contextCurrency;
            _contextCategory = contextCategory;
            _context = context;           
            _mapper = mapper;
        }        
     
        public async Task<IActionResult> Index()
        {
            var prodList = await _context.GetAllAsync();
            foreach (var item in prodList) {
                item.Img = CreateQrCode(item.Code);
            }

            return View(prodList);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _contextCategory.GetAllAsync(), "Id", "Name");
            ViewData["CurrencyId"] = new SelectList(await _contextCurrency.GetAllAsync(), "Id", "Name");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,CategoryId,CurrencyId")] ProductViewModel model)
        {
            if (ModelState.IsValid) {
                Product product = _mapper.Map<ProductViewModel, Product>(model);
                string code = null;
                do {
                    code = GenerateCode();                    
                } while (_dbContext.Product.Any(p => p.Code == code));

                product.Code = code;
                product.PriceBase = await RecountToBasePrice(model.Price, model.CurrencyId);

                if (!await _context.AddAsync(product)) {
                    return BadRequest("Something went wrong. Product is not added.");
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await _contextCategory.GetAllAsync(), "Id", "Name");
            ViewData["CurrencyId"] = new SelectList(await _contextCurrency.GetAllAsync(), "Id", "Name");
            return View(model);
        }               

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            var product = await _context.FindByIdAsync((int)id);

            if (product == null) {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(await _contextCategory.GetAllAsync(), "Id", "Name");
            ViewData["CurrencyId"] = new SelectList(await _contextCurrency.GetAllAsync(), "Id", "Name");
            ProductViewModel model = _mapper.Map<Product, ProductViewModel>(product);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,CategoryId,CurrencyId")] ProductViewModel model)
        {
            if (id != model.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                    var product = await _context.FindByIdAsync(id);
                    if (product == null) {
                        return NotFound();
                    }
                product.Name = model.Name;
                product.CategoryId = model.CategoryId;

                if (product.Price != model.Price || product.CurrencyId != model.CurrencyId) {
                    product.PriceBase = await RecountToBasePrice(model.Price, model.CurrencyId);
                    product.CurrencyId = model.CurrencyId;
                    product.Price = model.Price;
                }
                
                await _context.UpdateAsync(product);
              
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await _contextCategory.GetAllAsync(), "Id", "Name");
            ViewData["CurrencyId"] = new SelectList(await _contextCurrency.GetAllAsync(), "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            var model = await _context.FindByIdAsync((int)id);
            if (model == null) {
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
                return BadRequest("Something went wrong. Product is not deleted.");
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EntityExists(int id)
        {
            return (await _context.FindByIdAsync(id) != null);
        }

        private string GenerateCode(int length = 8)
        {
            StringBuilder code = new StringBuilder();
            List<int> list = new List<int>();
            Random random = new Random();
            int rnd;
            for (int i = 0; i < length; i++) {
                rnd = random.Next(0, 9);
                list.Add(rnd);
                code.Append(rnd.ToString());
            }
            
            return code.ToString();
        }

        private async Task<decimal> RecountToBasePrice(decimal price, int currencyId)
        {
            var prodCurrency = await _contextCurrency.FindByIdAsync(currencyId);
            if (prodCurrency == null) {
                throw new ApplicationException("Currency does not exist!");
            }
            return price / prodCurrency.Rate;
        }

        private static byte[] CreateQrCode(string content)
        {
            BarcodeWriter writer = new BarcodeWriter {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions {
                    Width = 100,
                    Height = 100,
                }
            };

            var qrCodeImage = writer.Write(content);

            using (var stream = new MemoryStream()) {
                qrCodeImage.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
