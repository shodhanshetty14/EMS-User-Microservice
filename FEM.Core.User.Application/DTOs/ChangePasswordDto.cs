using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM.Core.User.Application.DTOs
{
    public class ChangePasswordDto
    {
        public required string OldPassword { get; set; }
     
        public required string NewPassword { get; set; }
        
        [Compare(nameof(NewPassword))]
        public required string ConfirmNewPassword { get; set; }
    }
}
