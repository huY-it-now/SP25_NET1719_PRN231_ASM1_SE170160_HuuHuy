using GrowthTracking.Repository.Models;
using GrowthTracking.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GrowthTracking.APIServices.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UserService _userService;

        public UserController(IConfiguration config, UserService userService)
        {
            _config = config;
            _userService = userService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginReqeust request)
        {
            var user = _userService.Authenticate(request.email, request.password);

            if (user == null || user.Result == null)
                return Unauthorized();

            var token = GenerateJSONWebToken(user.Result);

            return Ok(token);
        }

        private string GenerateJSONWebToken(User systemUserAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                    , _config["Jwt:Audience"]
                    , new Claim[]
                    {
                new(ClaimTypes.Name, systemUserAccount.FullName),
                //new(ClaimTypes.Email, systemUserAccount.Email),
                new(ClaimTypes.Role, systemUserAccount.RoleCodeId.ToString()),
                    },
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public sealed record LoginReqeust(string email, string password);

        [HttpGet("GetAllDoctors")]
        [Authorize(Roles = "1")]
        public async Task<IEnumerable<User>> GetAllDoctors()
        {
            return await _userService.GetAllDoctors();
        }
    }
}
