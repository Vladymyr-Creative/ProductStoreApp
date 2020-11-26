using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStoreApp.ViewModels
{
    public class CurrencyCreateViewModel
    {
        public string Name { get; set; }

        [Required]
        public int Code { get; set; }
    }
}
