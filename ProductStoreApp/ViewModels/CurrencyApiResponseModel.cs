using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStoreApp.ViewModels
{
    public class CurrencyApiResponseModel
    {
        public int r030 { get; set; }
        public string txt { get; set; }
        public decimal rate { get; set; }
        public string cc { get; set; }        
    }
}
