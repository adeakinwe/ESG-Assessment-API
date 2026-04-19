using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ESG.Api.Data;
using ESG.Api.DTOs;
using ESG.Api.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ESG.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _authRepo;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthRepo authRepo, IConfiguration configuration)
        {
            _authRepo = authRepo;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("FirstName, Email and Password are required.");

            try
            {
                var user = await _authRepo.Register(request);

                return CreatedAtAction(nameof(Register), new { user.Id }, new { user.Id, user.FirstName, user.LastName, user.Email });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email and Password are required.");

            var user = await _authRepo.Login(request);

            if (user == null)
                return Unauthorized("Invalid credentials.");

            var token = GenerateJwtToken(user);

            return Ok(new AuthResponseDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = token
            });
        }

        private string GenerateJwtToken(AuthResponseDTO user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secret = jwtSettings.GetValue<string>("Key") ?? throw new InvalidOperationException("Jwt:Key must be configured");
            var issuer = jwtSettings.GetValue<string>("Issuer") ?? "ESGAPI";
            var audience = jwtSettings.GetValue<string>("Audience") ?? "ESGAPI";
            var expires = DateTime.UtcNow.AddHours(1);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}