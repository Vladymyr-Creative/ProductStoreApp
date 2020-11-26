using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProductStoreApp.Interfaces;
using ProductStoreApp.Models;
using ProductStoreApp.ViewModels;

namespace ProductStoreApp.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly AppDBContext _DBcontext;
        private readonly IRepository<Currency> _context;
        private readonly IConfiguration _conf;
        private readonly IMapper _mapper;

        public CurrencyController(
            AppDBContext DBcontext, 
            IRepository<Currency> context, 
            IConfiguration conf, 
            IMapper mapper
            )
        {
            _DBcontext = DBcontext;
            _context = context;
            _conf = conf;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Update() {
            var responseCurrencies = await GetCurrency();
            if (responseCurrencies == null) {
                return BadRequest("Something went wrong. Can't get exchange rate.");
            }
            var listMyCurrency = _DBcontext.Currency.ToList();
            if (listMyCurrency.Count == 0 ) {
                return BadRequest("You have no currency added!");
            }

            var baseCurrencyCode = _conf.GetValue<int>("BaseCurrencyCode");
            var nationalCurrencyCode = _conf.GetValue<int>("NationalCurrencyCode");
            decimal baseCurrencyRate = 1;
            decimal nationalCurrencyRate = responseCurrencies.FirstOrDefault(p=>p.r030 == baseCurrencyCode).rate;
            
            foreach (var myItem in listMyCurrency) {
                myItem.UpdatedAt = DateTime.Now;
                if (myItem.Code == baseCurrencyCode) {
                    myItem.Rate = baseCurrencyRate;
                    continue;
                }
                if (myItem.Code == nationalCurrencyCode) {
                    myItem.Rate = nationalCurrencyRate;
                    continue;
                }
                foreach (var respItem in responseCurrencies) {                    
                    if (respItem.r030 == myItem.Code) {
                        myItem.Rate = nationalCurrencyRate/respItem.rate;
                    }
                }
            }
            _DBcontext.Currency.UpdateRange(listMyCurrency);
            _DBcontext.SaveChanges();

            await UpdateBasePrice();

            return Ok("Success!");
        }


        private async Task UpdateBasePrice()
        {

            var products = _DBcontext.Product.ToList();
            var listMyCurrency = _DBcontext.Currency.ToList();
            var baseCurrencyCode = _conf.GetValue<int>("BaseCurrencyCode");

            foreach (var item in products) {
                if (item.Currency == null || item.Currency.Code == baseCurrencyCode) {                    
                    continue;
                }
               
                foreach (var currency in listMyCurrency) {
                    item.PriceBase = item.Price/currency.Rate;                    
                }                
            }

            _DBcontext.Product.UpdateRange(products);
            _DBcontext.SaveChanges();

        }

        public async Task<IActionResult> Create()
        {
            var currencies = _mapper.Map<IEnumerable<CurrencyApiResponseModel>, IEnumerable<CurrencyCreateViewModel>>(await GetCurrency());

            if (currencies == null) {
                return BadRequest("Something went wrong. Can't get exchange rate.");
            }

            ViewBag.Currencies = currencies;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CurrencyCreateViewModel model)
        {
            IEnumerable<CurrencyCreateViewModel> currencies = null;
            var myCurrency = _DBcontext.Currency.FirstOrDefault(c => c.Code == model.Code);
            if (myCurrency != null) {
                return RedirectToAction(nameof(Index));
            }
            var responseCurrencies = await GetCurrency();
            if (responseCurrencies == null) {
                return BadRequest("Something went wrong. Can't get exchange rate.");
            }

            if (ModelState.IsValid) {
                decimal nationalCurrencyRate = _DBcontext.Currency.FirstOrDefault(c => c.Code == _conf.GetValue<int>("NationalCurrencyCode")).Rate;
                CurrencyApiResponseModel existingCurrency = responseCurrencies.FirstOrDefault(c=>c.r030 == model.Code);
                Currency currency = _mapper.Map<CurrencyApiResponseModel, Currency>(existingCurrency);
                currency.UpdatedAt = DateTime.Now;
                currency.Rate = nationalCurrencyRate/currency.Rate;
                if (!await _context.AddAsync(currency)) {                    
                    return BadRequest("Something went wrong. Currency is not added.");
                }
                return RedirectToAction(nameof(Index));
            }

            currencies = _mapper.Map<IEnumerable<CurrencyApiResponseModel>, IEnumerable<CurrencyCreateViewModel>>(responseCurrencies);            
            ViewBag.Currencies = currencies;
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
            if (model.Code == _conf.GetValue<int>("BaseCurrencyCode") || model.Code == _conf.GetValue<int>("NationalCurrencyCode")) {
                return BadRequest("You can't delete this Currency! Currency is not deleted.");
            }
            if (!await _context.DeleteAsync(model)) {
                return BadRequest("Something went wrong. Currency is not deleted.");
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<HttpResponseMessage> GetCurrencyFromApiAsync()
        {
            HttpResponseMessage response = null;

            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("Accept", "aplication/json");
                response = await client.GetAsync(_conf.GetValue<string>("CurrencyApiUrl"));
            }
            return response;
        }

        private async Task<IEnumerable<CurrencyApiResponseModel>> GetCurrency()
        {
            var response = await GetCurrencyFromApiAsync();
            IEnumerable<CurrencyApiResponseModel> currencyList = null;

            if (response == null) {
                //TODO _logger NotFound("Resource is unavaiable.");                
                return currencyList;
            }
            string responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) {
                HttpContext.Response.StatusCode = (int)response.StatusCode;
                //TODO _logger //JsonConvert.DeserializeObject<object>(responseString); //TODO _logger
                //TODO _logger BadRequest("Something went wrong. Can't get exchange rate.");
                return currencyList;
            }

            try {                
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd.MM.yyyy" };
                currencyList = JsonConvert.DeserializeObject<IEnumerable<CurrencyApiResponseModel>>(responseString, dateTimeConverter);
            }
            catch (System.Exception e) {
                //TODO _logger => e
                //TODO _logger BadRequest("Something went wrong. Can't get exchange rate.");
                return currencyList;
                throw;
            }

            if (currencyList == null || currencyList.Count() == 0 || currencyList.First().r030 == default) {
                //TODO _logger => currencyList
                //TODO _logger BadRequest("Something went wrong. Can't get exchange rate.");
                return currencyList;

            }
            return currencyList;
        }

    }
}
