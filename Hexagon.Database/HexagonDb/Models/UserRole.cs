using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Hexagon.Database.HexagonDb.Models;

public class UserRole : IdentityUserRole<Guid>
{
    /// <summary>
    /// Gets or sets the the user that is linked to a role.
    /// </summary>
    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    /// <summary>
    /// Gets or sets the role that is linked to the user.
    /// </summary>
    [ForeignKey("RoleId")]
    public virtual Role Role { get; set; }
}