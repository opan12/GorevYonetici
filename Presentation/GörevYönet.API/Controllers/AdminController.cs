using GörevYönet.Application.Abstractions.Services;
using GörevYönet.Domain.Entitites;
using GörevYönet.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTasks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var tasks = await _taskService.GetTasks();
            var pagedTasks = tasks
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalTasks = tasks.Count();
            var totalPages = (int)Math.Ceiling(totalTasks / (double)pageSize);

            return Ok(new
            {
                Tasks = pagedTasks,
                Page = page,
                TotalPages = totalPages,
                TotalTasks = totalTasks
            });
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
