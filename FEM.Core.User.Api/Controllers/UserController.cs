using System.Security.Claims;
using FEM.Core.User.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DomainUser = FEM.Core.User.Domain.Entities;

namespace FEM.Core.User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<DomainUser.User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<DomainUser.User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleDto roleData)
        {

            if (!await _roleManager.RoleExistsAsync(roleData.Role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleData.Role));
                if (result.Succeeded)
                {
                    return Ok(new { message = "Role added successfully" });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest("Role already exists");
        }

        [Authorize]
        [HttpGet("user-profile")]
        public async Task<IActionResult> UserProfile()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByNameAsync(userName!);
            
            return Ok(user);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordData)
        {

            return Ok();
        }

    }
}
