using GraphQL.Types;
using Sistema.Proyecto;
using Test.EntityFramework.TestDbContext;

namespace Test.Web.GraphQL.Types
{
    public class UsuariosType : ObjectGraphType<CatUsuario>
    {
        private readonly IRepository<CatUsuario> _usuarioRepository;
        private readonly IRepository<CatRol> _rolRepository;

        public UsuariosType(IRepository<CatUsuario> usuarioRepository,
            IRepository<CatRol> rolRepository)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
            
            Name = "Usuario";
            Field(x => x.IdUsuario);
            Field(x => x.Nombre);
            Field(x => x.Email);
            Field(x => x.FolioUsuario);            
            Field(x => x.EstaActivo);
            Field<RolesType>("Rol",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "Id"}),
                resolve: context => 
                            _rolRepository.Get(context.Source.IdRol)
                            ,description: "Datos del rol");
            
        }
    }
}
