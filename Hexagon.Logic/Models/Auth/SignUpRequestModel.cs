using System.ComponentModel.DataAnnotations;
using Hexagon.Database.HexagonDb.Models;

namespace Hexagon.Logic.Models.Auth;

public class SignUpRequestModel
{
    [Required]
    public string AccountName { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
}