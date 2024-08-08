// Para manejar el proceso de inicio de sesión y generación de JWT.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using OrderBackend.Models;
using OrderBackend.Data;
using System.Text;
using System.Linq;

namespace OrderBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin login)
        {
            if (login == null || string.IsNullOrEmpty(login.NIT) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Invalid login request.");
            }

            var user = _context.Clientes.FirstOrDefault(c => c.NIT == login.NIT && c.Password == login.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(Cliente user)
        {
            if (user == null || string.IsNullOrEmpty(user.NIT))
            {
                throw new ArgumentNullException(nameof(user));
            }

            var claims = new[]
            {
                new Claim("nit", user.NIT),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.");
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured."),
                _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience is not configured."),
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
