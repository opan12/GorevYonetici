using GörevYönet.Application.Abstractions.Services;
using GörevYönet.Domain.Entitites;
using GörevYönet.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GörevYönet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ITaskService taskService, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _taskService = taskService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;


        }
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _taskService.GetTasks();
            return Ok(tasks);
        }
        [HttpPost("register")]
        public async Task<IActionResult> AdminRegister([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = "Admin",
                Firstname = "Admin",
                Lastname ="Admin",
                Email = "Admin",
            };

            var result = await _userManager.CreateAsync(user, "Admin.123"); // Şifre ile birlikte kullanıcının oluşturulması

            if (result.Succeeded)
            {
                // Kullanıcıya "User" rolünü atayın
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                await _userManager.AddToRoleAsync(user, "User");

                return Ok(new { Message = "Kayıt ve rol ataması başarılı.", AssignedRole = "User" });
            }

            return BadRequest(new { Message = "Kayıt başarısız.", Errors = result.Errors });
        }

    }
}
