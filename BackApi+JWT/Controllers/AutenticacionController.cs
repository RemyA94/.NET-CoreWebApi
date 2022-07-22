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
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly string _secretKey;
        public AutenticacionController(IConfiguration configuration)
        {
            _secretKey = configuration.GetSection("settings").GetSection("secretkey").ToString();
        }

        [HttpPost]
        [Route("Validar")]
        public IActionResult Validar([FromBody] Usuario request)
        {
            //como es una api de prueba simplemente predeterminamos la credenciales,
            if(request.Correo == "test@gmail.com" && request.Clave == "123")
            {
                var keyBytes = Encoding.ASCII.GetBytes(_secretKey);

                //creamos una solitud de permisos por el correo del usuario
                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Correo));

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
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "No autorizado" });

            }

        }
    }
}
 