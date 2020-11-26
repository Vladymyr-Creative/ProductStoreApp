using AutoMapper;
using ProductStoreApp.Models;
using ProductStoreApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStoreApp.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductViewModel, Product>();

            CreateMap<Product, ProductViewModel>();
        }
    }
}
