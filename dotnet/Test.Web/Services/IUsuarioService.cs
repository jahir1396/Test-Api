using Test.Web.Controllers.InputOutput;

namespace Test.Web.Services
{
    public interface IUsuarioService
    {
        Task<CrearUsuarioOutput> CrearUsuario(CrearUsuarioInput input);
        Task<LoginOutput> Login(LoginInput input);
        Task<ObtenerUsuariosOutput> ObtenerUsuarios();


        Task<int> OperacionMatematica(int valor1, int valor2);
    }
}
