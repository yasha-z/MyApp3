using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Session1
{
    [ApiController]//this tells that this class is a controller and it will handle HTTP requests
    [Produces("application/json")]//this tells that this controller will return JSON responses
    [Route("auth")]

    //controllerbase gives methods like ok(), notfound(), badrequest()
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;


//fake users for now baad me actual database se users ko fetch karenge
        private static readonly Dictionary<string, (string Password, string Role)> Users = new(StringComparer.OrdinalIgnoreCase)
        {
            ["admin"] = ("admin123", "Admin"),
            ["reader"] = ("reader123", "Reader")
        };

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous] //this endpoint can be accessed without authentication
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
        {
            if (!Users.TryGetValue(request.Username, out var user) || user.Password != request.Password)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var token = GenerateToken(request.Username, user.Role);//generate a token for the user
            //only if usernmae and pass are correct
            return Ok(new LoginResponse(token));
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult<object> Me()
        {
            //when jwt is validated phir usme se claims nikal ke we can use those claims to get the username and role of the user
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(new
            {
                Username = User.Identity?.Name,
                Role = User.FindFirstValue(ClaimTypes.Role),
                Claims = claims
            });
        }

        private string GenerateToken(string username, string role)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
//sign the token with the key using HMAC SHA256 algorithm taake agar kisi ne token 
// ko modify kiya hai to signature invalid hojaye
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Sub, username),//unique identifier for the user
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
