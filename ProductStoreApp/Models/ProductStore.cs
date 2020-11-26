using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStoreApp.Models
{
    public class ProductStore
    {
        [Key]
        [Required]
        public int ProductId { get; set; }

        [Key]
        [Required]
        public int StoreId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public Product Product { get; set; }
        public Store Store { get; set; }
    }
}
