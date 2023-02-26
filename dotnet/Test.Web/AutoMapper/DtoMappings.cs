using AutoMapper;
using Sistema.Proyecto;
using Test.Web.Controllers.Dto;
using Test.Web.Controllers.InputOutput;

namespace Test.Web.AutoMapper
{
    public class DtoMappings : Profile
    {
        public DtoMappings()
        {
            // AutoMapTo
            CreateMap<CrearUsuarioInput, CatUsuario>();


            // AutoMapFrom
            CreateMap<CatUsuario, UsuarioDto>()
                .ForMember(d => d.Rol, options => options.MapFrom(s => s.Rol.Nombre));
            //CreateMap<CatCategoria, CategoriaDto>();
        }
    }
}
