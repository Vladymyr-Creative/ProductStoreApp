using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductStoreApp.Interfaces;
using ProductStoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ProductStoreApp
{
    public class CurrencyUpdateFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var DBContext = context.HttpContext.RequestServices.GetService<AppDBContext>();
            var currency = DBContext.Currency.FirstOrDefault();
            bool  needUpdate = false;
            if (currency != null && currency.UpdatedAt.Date != DateTime.Now.Date) {
                needUpdate = true;
            }

            context.HttpContext.Response.Cookies.Append("NeedUpdateCurrency", needUpdate.ToString());
            await next();
        }
    }
}
