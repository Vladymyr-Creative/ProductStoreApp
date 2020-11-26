using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStoreApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, Double.PositiveInfinity)]
        public decimal PriceBase { get; set; }

        [Required]
        [Range(0, Double.PositiveInfinity)]
        public decimal Price { get; set; }

        [NotMapped]
        public byte[] Img { get; set; }

        public int? CategoryId { get; set; }

        public int? CurrencyId { get; set; }


        public Currency Currency { get; set; }
        public Category Category { get; set; }
    }
}
