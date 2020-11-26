using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductStoreApp.Models
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]        
        public int Code { get; set; }

        [Required]
        [Range(0, Double.PositiveInfinity)]
        public decimal Rate { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public List<Product> Products { get; set; }
    }
}
