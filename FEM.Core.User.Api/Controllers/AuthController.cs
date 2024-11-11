using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FEM.Core.User.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using DomainUser = FEM.Core.User.Domain.Entities;

namespace FEM.Core.User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<DomainUser.User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<DomainUser.User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerData)
        {
            DomainUser.User user = new DomainUser.User
            {
                UserName = registerData.UserName,
                PhoneNumber = registerData.PhoneNumber,
                Email = registerData.Email,
                OrganisationId = Guid.NewGuid(),
            };
            
            var result = await _userManager.CreateAsync(user, registerData.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            var roleResult = await _userManager.AddToRoleAsync(user, registerData.Role);
            
            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return Created("User Created Successfully", user);

        }
        

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginData)
        {
            var user = await _userManager.FindByEmailAsync(loginData.Email);
            if (user == null)
                return NotFound("User not found");

            if (!await _userManager.CheckPasswordAsync(user, loginData.Password))
                return BadRequest("Invalid Password!");


            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(double.Parse(_config["Jwt:ExpiryMinutes"]!)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)),
                SecurityAlgorithms.HmacSha256));

            string encryptedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = $"Bearer {encryptedToken}",
            });
        }

    }
}
