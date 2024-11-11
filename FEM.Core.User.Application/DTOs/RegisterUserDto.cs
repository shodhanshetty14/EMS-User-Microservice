using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM.Core.User.Application.DTOs
{
    /// <summary>
    /// Register User Data Format
    /// </summary>
    public class RegisterUserDto
    {
        [Required(ErrorMessage ="Username is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required"), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required is required")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required"), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required, AllowedValues("SuperAdmin", "Admin", "User")]
        public string Role { get; set; } = string.Empty;
    }
}
