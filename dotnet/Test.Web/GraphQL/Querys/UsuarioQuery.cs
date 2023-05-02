using AutoMapper;
using GraphQL;
using GraphQL.Introspection;
using GraphQL.Types;
using Sistema.Proyecto;
using System.Reactive.Disposables;
using Test.Web.GraphQL.Types;
using Test.Web.Services;

namespace Test.Web.GraphQL.Querys
{
    public class UsuarioQuery : ObjectGraphType<CatUsuario>
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRepository<CatUsuario> _usuarioRepository;
        private readonly IRepository<CatRol> _rolRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioQuery(
            IRepository<CatUsuario> usuarioRepository,
            IRepository<CatRol> rolRepository,
            IUsuarioService usuarioService,
            IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
            _unitOfWork = unitOfWork;
            _usuarioService = usuarioService;

            FieldAsync<ListGraphType<UsuariosType>>(
                "listarUsuarios",
                arguments: new QueryArguments(new List<QueryArgument>
                {
                    new QueryArgument<IdGraphType>
                    {
                        Name = "id"
                    },
                    new QueryArgument<StringGraphType>
                    {
                        Name = "nombre"
                    },
                    new QueryArgument<StringGraphType>
                    {
                        Name = "email"
                    }
                }),
                resolve: async context =>
                {
                    var usuarioId = context.GetArgument<int?>("id");
                    var usuarioNombre = context.GetArgument<string>("nombre");
                    var usuarioEmail = context.GetArgument<string>("email");

                    var res = await _usuarioRepository.GetAllListAsync(s => (usuarioId.HasValue ? s.IdUsuario == usuarioId : true)
                            && (!string.IsNullOrEmpty(usuarioNombre) ? s.Nombre.Contains(usuarioNombre): true)
                            && (!string.IsNullOrEmpty(usuarioEmail) ? s.Email.Contains(usuarioEmail): true));

                    
                    return await _usuarioRepository.GetAllListAsync();
                });            
        }

    }
}
