using AutoMapper;
using DBAccess.Data;
using Models;

namespace RfToDoAPI.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Meta, MetaModel >().ReverseMap();
            CreateMap<Tarea, TareaModel>().ReverseMap();
        }
    }
}
