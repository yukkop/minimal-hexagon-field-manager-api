using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hexagon.Database.HexagonDb.Models;

public class User : IdentityUser<Guid>
{
#pragma warning disable 0114, 0618
    public string? PasswordHash
    {
        get => base.PasswordHash;
        set => base.PasswordHash = value;
    }
    
    [NotMapped] 
    [Obsolete("UserName is deprecated, please use AccountName instead.")]
    [EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    // ReSharper disable once MemberCanBePrivate.Global
    public override string UserName
    {
        get => base.UserName;
        set => base.UserName = value;
    }

    /// <summary>
    /// Gets or sets the unic account name to this user;
    /// </summary>
    [Column("AccountName")]
    [Required]
    public string AccountName
    {
        get => UserName;
        set
        {
            UserName = value;
            NormalizedUserName = value.ToUpper();
        }
    }

    [NotMapped]
    [Obsolete("NormalizedUserName is deprecated, please use NormalizedAccountName instead.")]
    [EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    // ReSharper disable once MemberCanBePrivate.Global
    public string NormalizedUserName
    {
        get => base.NormalizedUserName;
        set => base.NormalizedUserName = value;
    }

    [Column("NormalizedAccountName")]
    public string NormalizedAccountName
    {
        get => NormalizedUserName;
        set
        {
            NormalizedUserName = value;
        }
    }


    [NotMapped]
    [Obsolete("Email is deprecated, please use EmailAddress instead.")]
    [EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    // ReSharper disable once MemberCanBePrivate.Global
    public string Email
    {
        get => base.Email;
        set => base.Email = value;
    }

    /// <summary>
    /// Gets or sets the email address to this user;
    /// </summary>
    [Column("EmailAddress")]
    public string EmailAddress
    {
        get => Email;
        set
        {
            Email = value;
            NormalizedEmail = value.ToUpper();
        }
    }

    [NotMapped]
    [Obsolete("NormalizedEmail is deprecated, please use NormalizedEmailAddress instead.")]
    [EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    // ReSharper disable once MemberCanBePrivate.Global
    public string NormalizedEmail
    {
        get => base.NormalizedEmail;
        set => base.NormalizedEmail = value;
    }

    [Column("NormalizedEmailAddress")]
    public string NormalizedEmailAddress {
        get => NormalizedEmail;
        set
        {
            NormalizedEmail = value;
        }
    }

    [NotMapped]
    [Obsolete("EmailConfirmed is deprecated, please use EmailAddressConfirmed instead.")]
    [EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    // ReSharper disable once MemberCanBePrivate.Global
    public bool EmailConfirmed
    {
        get => base.EmailConfirmed;
        set => base.EmailConfirmed = value;
    }

    /// <summary>
    /// Gets or sets the email address to this user;
    /// </summary>
    [Column("EmailAddressConfirmed")]
    public bool EmailAddressConfirmed
    {
        get => EmailConfirmed;
        set => EmailConfirmed = value;
    }
    
#pragma warning restore 0114, 0618

    public Guid? ProfileId { get; set; }
    [ForeignKey("ProfileId")]
    public Profile? Profile { get; set; }
}
