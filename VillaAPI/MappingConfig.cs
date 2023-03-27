using AutoMapper;
using VillaAPI.Models;
using VillaAPI.Models.DTO;

namespace VillaAPI
{
    public class MappingConfig:Profile

    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
            CreateMap<Villa,VillaCreateDTO>().ReverseMap();
        }
    }
}
