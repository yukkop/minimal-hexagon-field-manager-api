using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hexagon.Database.HexagonDb.Models;

public class Profile: BaseModel
{
#pragma warning disable CS8618
    private string _username;
#pragma warning restore CS8618
    
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
#pragma warning disable CS0618
            _normalizedUsername = value.ToUpper();
#pragma warning restore CS0618
        }
    }

    [Column("NormalizedUsername")]
    [Required]
    [Obsolete("Using this is not safe and may lead to errors. Use NormalizedUsername")]
    [EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    // ReSharper disable once MemberCanBePrivate.Global
#pragma warning disable CS8618 // "Required" must control null 
    public string _normalizedUsername { get; set; }
#pragma warning restore CS8618

    /// <summary>
    /// Normalized username for faster search
    /// Normalized to uppercase?
    /// </summary>
    [NotMapped]
#pragma warning disable CS0618 // is exception for _normalizedUsername usage
    public string NormalizedUsername => _normalizedUsername;
#pragma warning restore CS0618
}