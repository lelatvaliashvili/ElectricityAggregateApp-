using AutoMapper;
using Electricity.Domain.Entities;
using Electricity.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electricity.Infrastructure.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ElectricityData, ElectricityConsumptionAggregate>()
                    .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Regions))
                    .ForMember(dest => dest.TotalPositive, opt => opt.MapFrom(src => src.PPlus))
                    .ForMember(dest => dest.TotalNegative, opt => opt.MapFrom(src => src.PMinus))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ReverseMap();
        }
    }
}
