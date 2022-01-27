using AutoMapper;
using ParkiAPI.Dtos;
using ParkiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkiAPI.Mapper
{
    public class ParkiMapping:Profile
    {

        public ParkiMapping()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<NationalParkDto, NationalPark>()
                .ForMember(x => x.Picture, options => options.Ignore());
            CreateMap<Trail, TrailDto>().ReverseMap();
            CreateMap<Trail, TrailCreateDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap();
        }
        
    }
}
