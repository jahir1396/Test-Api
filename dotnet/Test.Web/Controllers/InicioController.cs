using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema.Proyecto;

namespace Test.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InicioController : ControllerBase
    {
        [HttpGet]
        public string Inicio()
        {
            return "Welcome to Docker and Kubernetes World";
        }
    }
}
