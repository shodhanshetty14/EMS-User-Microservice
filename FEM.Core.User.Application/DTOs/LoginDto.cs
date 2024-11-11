using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM.Core.User.Application.DTOs
{
    /// <summary>
    /// Login User Data Format 
    /// </summary>
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required"), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required is required")]
        public string Password { get; set; } = string.Empty;
    }
}
