using Hexagon.Database.HexagonDb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hexagon.Database.HexagonDb;

public class HexagonContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public HexagonContext(DbContextOptions<HexagonContext> options): base(options) {}
    
    public DbSet<Profile> Profiles { get; set; }

    public override string ToString()
    {
        return base.ToString();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        modelBuilder.Entity<User>().Ignore(user => user.UserName);
        modelBuilder.Entity<User>().Ignore(user => user.Email);
        modelBuilder.Entity<User>().Ignore(user => user.EmailConfirmed);
        modelBuilder.Entity<User>().Ignore(user => user.NormalizedEmail);
        modelBuilder.Entity<User>().Ignore(user => user.NormalizedUserName);
    }
    
}