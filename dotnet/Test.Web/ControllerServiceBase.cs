using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Sistema.Proyecto;
using Sistema.Proyecto.Comun;
using System.Net;
using System.Security.Claims;

namespace Test.Web
{
    public class ControllerServiceBase : ControllerBase
    {
        public readonly InformacionUsuario InformacionUsuario;
        private readonly IHttpContextAccessor _httpContextAccessor;        
        private readonly IRepository<CatUsuario> _catUsuarioRepository;        

        public ControllerServiceBase(IHttpContextAccessor httpContextAccessor, IRepository<CatUsuario> catUsuarioRepository)
        {
            string loginName;
            _httpContextAccessor = httpContextAccessor;
            _catUsuarioRepository = catUsuarioRepository;

            if (_httpContextAccessor.HttpContext.Request.Path.Value.Contains("Usuarios/Login") || _httpContextAccessor.HttpContext.Request.Path.Value.Contains("Usuarios/CrearUsuario"))
            {
                InformacionUsuario = new InformacionUsuario
                {
                    IdUsuario = 1,
                    FolioUsuario = "JAHIR-412-2023",
                    Email = "jahir.ilv13@gmail.com",
                    IdRol = 1
                };
            }
            else
            {
                var mIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                loginName = mIdentity.Claims.FirstOrDefault(x => x.Type == "FolioUsuario").Value;

                if (loginName != null)
                {
                    InformacionUsuario = new InformacionUsuario
                    {
                        FolioUsuario = loginName
                    };
                }

                var mUsuario = _catUsuarioRepository.FirstOrDefault(u => u.FolioUsuario.Equals(loginName) && u.EstaActivo);

                if (mUsuario == null)
                    throw new AuthorizationException(HttpStatusCode.Unauthorized, "El usuario no está registrado, o activo, o no se encuentra autorizado, favor de revisar la información");
                else
                {
                    InformacionUsuario.Nombre = mUsuario.Nombre;
                    InformacionUsuario.FolioUsuario = mUsuario.FolioUsuario;
                    InformacionUsuario.Email = mUsuario.Email;
                    InformacionUsuario.IdRol = mUsuario.IdRol;
                }
            }
        }
    }
}
