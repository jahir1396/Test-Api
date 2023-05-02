using AutoMapper;
using GraphQL;
using GraphQL.Types;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Tokens;
using Sistema.Proyecto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Test.EntityFramework.TestDbContext;
using Test.Web.Controllers;
using Test.Web.Controllers.InputOutput;
using Test.Web.GraphQL.Types;
using Test.Web.Services;

namespace Test.Web.GraphQL.Querys
{
    public class UsuarioMutation : ObjectGraphType
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRepository<CatCategoria> _categoriaRepository;
        private readonly IRepository<CatUsuario> _catUsuariosRepository;
        private readonly IRepository<CatRol> _catRolRepository;
        private readonly IMapper _objectMapper;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IMapper _objectMapper;
        public UsuarioMutation(IUsuarioService usuarioService,
            IMapper objectMapper,
            IRepository<CatCategoria> categoriaRepository,
            IHttpContextAccessor httpContextAccessor,
            TestDbContext context,
            IRepository<CatRol> catRolRepository,
            IRepository<CatUsuario> catUsuariosRepository,
            IUnitOfWork unitOfWork,
            IConfiguration config)
        {
            _usuarioService = usuarioService;
            _categoriaRepository = categoriaRepository;
            _unitOfWork = unitOfWork;
            _objectMapper = objectMapper;
            _config = config;
            _catUsuariosRepository = catUsuariosRepository;
            _catRolRepository = catRolRepository;

            FieldAsync<CrearUsuarioOutputType>(
                "crearUsuario",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UsuarioInputType>>
                {
                    Name = "usuarios"
                }),
                resolve: async context =>
                {
                    var input = context.GetArgument<CrearUsuarioInput>("usuarios");

                    //var input = _objectMapper.Map<CrearUsuarioInput>(usuario);

                    var result = await _usuarioService.CrearUsuario(input);

                    return result;
                });

            FieldAsync<CategoriaType>(
            "crearActualizarCategoria",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CategoriaInputType>>
                {
                    Name = "categorias"
                }),
                resolve: async context =>
                {
                    var categoria = context.GetArgument<CatCategoria>("categorias");

                    if (categoria.IdCategoria == 0)
                        await _categoriaRepository.InsertAsync(categoria);
                    else
                        _categoriaRepository.Update(categoria);

                    _unitOfWork.SaveChanges();
                    return categoria;
                });

            FieldAsync<LoginUsuarioOutputType>(
                "login",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UsuarioLoginType>>
                {
                    Name = "login"
                }),
                resolve: async context =>
                {
                    var input = context.GetArgument<LoginInput>("login");

                    var result = await _usuarioService.Login(input);

                    return result;
                });

            FieldAsync<ListGraphType<UsuariosType>>(
                "cambiarRolUsuarios",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CambiarRolUsuariosInputType>>
                {
                    Name = "usuarios"
                }),
                resolve: async context =>
                {
                    var mUsuarios = new List<CatUsuario>();
                    var input = context.GetArgument<CambiarRolUsuariosInput>("usuarios");

                    var mRol = await _catRolRepository.GetAsync(input.IdRol);


                    if (mRol != null)
                    {
                        input.IdUsuarios.ForEach(async s =>
                        {
                            var mUsuario = await _catUsuariosRepository.GetAsync(s);

                            mUsuario.IdRol = input.IdRol;

                            _catUsuariosRepository.Update(mUsuario);

                            mUsuarios.Add(mUsuario);
                        });
                    }

                    _catRolRepository.Update(mRol);

                    return mUsuarios;
                });

            FieldAsync<CalculatorOutputType>(
                "calculator",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CalculatorInputType>>
                {
                    Name = "Valores"
                }),
                resolve: async context =>
                {
                    var output = new CalculatorOutput();
                    var input = context.GetArgument<CalculatorInput>("Valores");

                    var resultado = await _usuarioService.OperacionMatematica(input.Valor1,input.Valor2);

                    output.Resultado = resultado;

                    return output;
                });
        }
    }
}
