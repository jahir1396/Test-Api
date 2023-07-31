using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Test.Web.Middleware
{
    public class ValidaJwt
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public ValidaJwt(RequestDelegate next,
            IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            string loginName = string.Empty;

            if ((context.Request.ContentLength > 0 && context.Request.ContentLength != null) || context.Request.Path.Value.Length > 4)
            {
                try
                {
                    if(context.Request.Path.Value.Contains("Usuarios/Login") || context.Request.Path.Value.Contains("Usuarios/CrearUsuario") || context.Request.Path.Value.Contains("api/Inicio") || context.Request.Path.Value.Contains("Roles/ObtenerRoles"))
                        await _next.Invoke(context);
                    else
                    {
                        string token = string.Empty;
                        if (!TryRetrieveToken(context, out token))
                            throw new AuthorizationException(System.Net.HttpStatusCode.Unauthorized, "Usuario no autorizado");

                        var now = DateTime.UtcNow;
                        var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(_config["Jwt:Key"]));

                        SecurityToken securityToken;
                        JwtSecurityTokenHandler handler = new();

                        var validationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = false,
                            LifetimeValidator = this.LifeTimeValidator,
                            IssuerSigningKey = securityKey
                        };

                        context.User = handler.ValidateToken(token, validationParameters, out securityToken);

                        var mIdentity = (ClaimsIdentity)context.User.Identity;
                        loginName = mIdentity.Claims.FirstOrDefault(x => x.Type.Equals("FolioUsuario")).Value;

                        await _next.Invoke(context);
                    }
                }
                catch (AuthorizationException ex)
                {
                    throw new AuthorizationException(HttpStatusCode.Unauthorized, ex.Message);
                }
                catch (SecurityTokenValidationException ex)
                {
                    throw new SecurityTokenValidationException(ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private static bool TryRetrieveToken(HttpContext context, out string token)
        {
            token = null;
            StringValues authzHeaders;
            if (!context.Request.Headers.TryGetValue("Authorization", out authzHeaders) || authzHeaders.Count > 1)
            {
                return false;
            }

            var bearerToken = authzHeaders[0];
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

            return true;
        }

        public bool LifeTimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }

    public static class ValidaJwtExtension
    {
        public static IApplicationBuilder UseValidaJwt(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidaJwt>();
        }
    }
}
