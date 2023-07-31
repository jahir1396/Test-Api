using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sistema.Proyecto;
using Test.EntityFramework.TestDbContext;

namespace Test.Web.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerServiceBase
	{
        private readonly IRepository<CatRol> _catRolRepository;
        private readonly IRepository<CatUsuario> _catUsuarioRepository;
        private readonly IMapper _objectMapper;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public RolesController(IHttpContextAccessor httpContextAccessor,
            TestDbContext context,
            IRepository<CatRol> catRolRepository,
            IRepository<CatUsuario> catUsuarioRepository,
            IMapper objectMapper,
            IUnitOfWork unitOfWork,
            IConfiguration config) : base(httpContextAccessor, catUsuarioRepository)
        {
            _catUsuarioRepository = catUsuarioRepository;
            _objectMapper = objectMapper;
            _config = config;
            _unitOfWork = unitOfWork;
            _catRolRepository = catRolRepository;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ObtenerRolesOutput> ObtenerRoles()
        {
            var mRoles = await _catRolRepository.GetAllListAsync();

            return new ObtenerRolesOutput
            {
                Roles = _objectMapper.Map<List<RolDto>>(mRoles)
            };
        }
	}
}

