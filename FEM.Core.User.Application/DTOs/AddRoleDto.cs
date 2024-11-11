using System.ComponentModel.DataAnnotations;

namespace FEM.Core.User.Application.DTOs
{
    public class AddRoleDto
    {
        [Required, AllowedValues("Admin", "SuperAdmin", "User")]
        public string Role { get; set; } = string.Empty;
    }
}
