using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using GörevYönet.Models.ViewModel;
using GörevYönet.Domain.Entitites;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace GörevYönetici.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            // Tüm mevcut rolleri al
            var roles = await _roleManager.Roles.ToListAsync();

            // Rolleri listele
            var roleList = new List<string>();
            foreach (var role in roles)
            {
                roleList.Add(role.Name); // Sadece rol adını ekle
            }

            return Ok(roleList); // Rolleri döndür
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = model.UserName,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password); // Şifre ile birlikte kullanıcının oluşturulması

            if (result.Succeeded)
            {
                // Kullanıcıya "User" rolünü atayın
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }

                await _userManager.AddToRoleAsync(user, "User");

                return Ok(new { Message = "Kayıt ve rol ataması başarılı.", AssignedRole = "User" });
            }

            return BadRequest(new { Message = "Kayıt başarısız.", Errors = result.Errors });
        }

    }
}