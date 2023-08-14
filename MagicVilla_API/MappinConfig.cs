using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API
{
    public class MappinConfig : Profile
    // Profile es una clase que existe en el paquete AutoMapper
    {
        public MappinConfig()
        {
            CreateMap<Villa, VillaDto>();
            // 1er parameto es la Fuente y 2do parametro es el destino
            CreateMap<VillaDto, Villa>();
            // Ahora hacemos lo inverso

            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            // Esto es lo mismo que lo de arriba pero simplificado usando ReverseMap().
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
        }
    }
}
