using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory; 
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GörevYönet.Models.ViewModel;
using GörevYönet.Domain.Entitites;

namespace GörevYönet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;

        public TokenController(UserManager<User> userManager, IConfiguration configuration, IMemoryCache cache)
        {
            _userManager = userManager;
            _configuration = configuration;
            _cache = cache; 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _cache.TryGetValue(model.Username, out User cachedUser); 

            if (cachedUser == null)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
                {
                    return BadRequest("Invalid username or password.");
                }

                var token = await GenerateJwtToken(user);

                _cache.Set(model.Username, user, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30) 
                });

                cachedUser = user; 
            }

            var tokenResult = GenerateJwtToken(cachedUser);
            return Ok(new { Token = tokenResult.Result }); 
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "defaultRole")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}