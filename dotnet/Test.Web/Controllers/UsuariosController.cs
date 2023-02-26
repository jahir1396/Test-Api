using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Tokens;
using Sistema.Proyecto;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using Test.EntityFramework.TestDbContext;
using Test.Web.Controllers.Dto;
using Test.Web.Controllers.InputOutput;

namespace Test.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerServiceBase
    {
        private readonly IRepository<CatUsuario> _catUsuariosRepository;
        private readonly IRepository<CatRol> _catRolRepository;
        private readonly IMapper _objectMapper;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public UsuariosController(IHttpContextAccessor httpContextAccessor, 
            TestDbContext context,
            IRepository<CatRol> catRolRepository,
            IRepository<CatUsuario> catUsuariosRepository,
            IMapper objectMapper,
            IUnitOfWork unitOfWork,
            IConfiguration config
            ) : base(httpContextAccessor, catUsuariosRepository)
        {
            _catUsuariosRepository = catUsuariosRepository;
            _objectMapper = objectMapper;
            _config = config;
            _unitOfWork = unitOfWork;
            _catRolRepository = catRolRepository;
        }


        [HttpPost]
        [Route("CrearUsuario")]
        public async Task<CrearUsuarioOutput> CrearUsuario(CrearUsuarioInput input)
        {
            input.Email = input.Email.ToLower();

            if (await _catUsuariosRepository.CountAsync(u => u.Email.Equals(input.Email)) > 0)
                throw new AuthorizationException(System.Net.HttpStatusCode.BadRequest, "El email ya existe");

            string anioActual = DateTime.Now.ToString("yyyy");
            var random = new Random();
            string nombre = input.Nombre.ToUpper().Substring(0, 5).Trim(' ');

            string folio = string.Format($"{nombre}-{random.Next(0, 1000)}-{anioActual}");

            var mUsuario = _objectMapper.Map<CatUsuario>(input);

            CrearPasswordHash(input.Password, out byte[] passwordHash, out byte[] passwordSalt);

            mUsuario.FolioUsuario = folio;
            mUsuario.EstaActivo = true;
            mUsuario.PasswordHash = passwordHash;
            mUsuario.PasswordSalt = passwordSalt;

            await _catUsuariosRepository.InsertAsync(mUsuario);

            _unitOfWork.SaveChanges();

            return new CrearUsuarioOutput
            {
                Mensaje = "Operación Exitosa"                
            };

        }


        [HttpPost]
        [Route("Login")]
        public async Task<LoginOutput> Login(LoginInput input)
        {
            input.Email = input.Email.ToLower();

            var mUsuario = await _catUsuariosRepository.FirstOrDefaultAsync(u => u.Email.Equals(input.Email));
            var mRol = await _catRolRepository.GetAsync(mUsuario.IdRol);
            mUsuario.Rol = mRol;

            if(await _catUsuariosRepository.CountAsync(u => u.Email.Equals(input.Email)) <= 0 || mUsuario == null)
                throw new Exception("El usuario no existe");


            if (!VerificarPasswordHash(input.Password, mUsuario.PasswordHash, mUsuario.PasswordSalt))
                throw new Exception("La contraseña es incorrecta");

            var mClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, mUsuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, mUsuario.Nombre),
                new Claim(ClaimTypes.UserData, mUsuario.FolioUsuario),
                new Claim(ClaimTypes.Role, mUsuario.Rol.Nombre),
                new Claim(ClaimTypes.Email, mUsuario.Email),
                new Claim("Id", mUsuario.IdUsuario.ToString()),
                new Claim("FolioUsuario", mUsuario.FolioUsuario),
                new Claim("NombreUsuario", mUsuario.Nombre),
                new Claim("Rol", mUsuario.Rol.Nombre),
                new Claim("Email", mUsuario.Email)
            };

            return new LoginOutput
            {
                Mensaje = "Operación Exitosa",
                Usuario = mUsuario.Nombre,
                Token = GenerateToken(mClaims),
                Expires = DateTime.Now.AddDays(7)
            };

        }


        [HttpPost]
        [Route("ObtenerUsuarios")]
        public async Task<ObtenerUsuariosOutput> ObtenerUsuarios()
        {
            var mUsuarios = await _catUsuariosRepository.GetAllListAsync();

            return new ObtenerUsuariosOutput
            {
                Usuarios = _objectMapper.Map<List<UsuarioDto>>(mUsuarios)
            };
        }

        private string GenerateToken(List<Claim> claims)
        {
            var mKey = _config["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds,
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerificarPasswordHash(string password, byte[] passwordHashAlmacenado, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var passwordHashNuevo = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHashAlmacenado).SequenceEqual(new ReadOnlySpan<byte>(passwordHashNuevo));
            }
        }



    }
}
