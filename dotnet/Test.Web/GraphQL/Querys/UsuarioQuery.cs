using GraphQL;
using GraphQL.Types;
using Sistema.Proyecto;
using Test.Web.GraphQL.Types;

namespace Test.Web.GraphQL.Querys
{
    public class UsuarioQuery : ObjectGraphType<CatUsuario>
    {
        private readonly IRepository<CatUsuario> _usuarioRepository;

        public UsuarioQuery(
            IRepository<CatUsuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;

            FieldAsync<ListGraphType<UsuariosType>>(
                "usuarios",
                arguments: new QueryArguments(new List<QueryArgument>
                {
                    new QueryArgument<IdGraphType>
                    {
                        Name = "id"
                    }
                }),
                resolve: async context =>
                {
                    var usuarioId = context.GetArgument<int?>("id");

                    if (usuarioId.HasValue)
                    {
                        var res = await _usuarioRepository.GetAllListAsync(u => u.IdUsuario == usuarioId);
                        return res;
                    }
                    return await _usuarioRepository.GetAllListAsync();
                });
        }

    }
}
