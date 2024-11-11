
using Microsoft.AspNetCore.Identity;

namespace FEM.Core.User.Domain.Entities;
/// <summary>
/// 
/// </summary>
/// <param name="Id">User Id</param>
/// <param name="Username">the username of user</param>
/// <param name="Email">Email id of user</param>
/// <param name="PhoneNumber">Phone number of user</param>
/// <param name="Password">Hashed Password of User</param>
/// <param name="OrganisationId">Foreign Key to a Organisation Table</param>
public class User : IdentityUser
{
    public Guid OrganisationId { get; set; }
}


