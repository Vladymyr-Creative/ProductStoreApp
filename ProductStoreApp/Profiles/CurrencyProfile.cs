using AutoMapper;
using ProductStoreApp.Models;
using ProductStoreApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStoreApp.Profiles
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<CurrencyApiResponseModel, CurrencyCreateViewModel>()
                .ForMember("Code", opt => opt.MapFrom(c => c.r030))
                .ForMember("Name", opt => opt.MapFrom(c => c.cc + " - " + c.txt));

            CreateMap<CurrencyApiResponseModel, Currency>()
                .ForMember("Code", opt => opt.MapFrom(c => c.r030))
                .ForMember("Name", opt => opt.MapFrom(c => c.cc + " - " + c.txt));

        }
    }
}
