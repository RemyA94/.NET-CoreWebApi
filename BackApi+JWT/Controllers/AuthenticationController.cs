using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using BackApi_JWT.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Cors;

namespace BackApi_JWT.Controllers
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly string _secretKey;
        public AuthenticationController(IConfiguration configuration)
        {
            _secretKey = configuration.GetSection("settings").GetSection("secretkey").ToString();
        }

        [HttpPost]
        [Route("validate")]
        public IActionResult validate([FromBody] User request)
        {
            //como es una api de prueba simplemente predeterminamos la credenciales,
            if(request.Mail == "test@gmail.com" && request.Key == "123")
            {
                var keyBytes = Encoding.ASCII.GetBytes(_secretKey);

                //creamos una solitud de permisos por el correo del usuario
                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Mail));

                //configuramos el token
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                //hacemos lectura del token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                //obtener nuestro token creado
                string tokenCreado = tokenHandler.WriteToken(tokenConfig);

                return StatusCode(StatusCodes.Status200OK, new { token = tokenCreado });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "Not authorized" });

            }

        }
    }
}
 