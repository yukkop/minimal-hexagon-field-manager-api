using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Hexagon.Database.HexagonDb;
using Hexagon.Database.HexagonDb.Models;
using Hexagon.Logic.Exceptions;
using Hexagon.Logic.Models;
using Hexagon.Logic.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Hexagon.Logic.Logic;

public interface IUserLogic : IBaseLogic
{
    Task<Guid> SignUp(SignUpRequestModel model);
    Task<TokenModel> SignInByLogin(SignInRequestModel model);
}

public class UserLogic : IUserLogic
{
    public int? UserId { get; set; }

    private readonly HexagonContext _hexagonContext;

    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;

    private readonly IConfiguration _configuration;

    private readonly IMapper _mapper;

    public UserLogic(
        HexagonContext hexagonContext, 
        UserManager<User> userManager, 
        SignInManager<User> signInManager,
        RoleManager<Role> roleManager,
        IConfiguration configuration,
        IMapper mapper)
    {
        _hexagonContext = hexagonContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<Guid> SignUp(SignUpRequestModel model)
    {
        var user = await _hexagonContext.Users.FirstOrDefaultAsync(x =>
            x.AccountName.ToUpper() == model.AccountName.ToUpper());
        if (user != null)
        {
            throw new HexagonException("Пользователь уже зарегестрирован");
        }

        var alg = SHA512.Create();
        alg.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
        user = new User
        {
            AccountName = model.AccountName,
            SecurityStamp = Guid.NewGuid().ToString(),
            EmailAddress = model.Email
        };

        await _hexagonContext.Users.AddAsync(user);
        await _hexagonContext.SaveChangesAsync();
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
        await _hexagonContext.SaveChangesAsync();

        return user.Id;
    }

    public async Task<TokenModel> SignInByLogin(SignInRequestModel model)
    {
        var ret = new TokenModel();
        var user = await _hexagonContext.Users
            .Where(x => x.NormalizedAccountName == model.Login.ToUpper() ||
                        x.NormalizedEmailAddress == model.Login.ToUpper())
            .FirstOrDefaultAsync();

        if (user == null) return ret;
        
        var result =
            await _signInManager.PasswordSignInAsync(user.NormalizedEmailAddress, model.Password, false, false);
        if (result.Succeeded)
            return await SignIn(user);

        return ret;
    }
    
    // public virtual string NormalizeName(string name)
    //        => (KeyNormalizer == null) ? name : KeyNormalizer.NormalizeName(name);
    
    private async Task<TokenModel> SignIn(User user)
        {
            var ret = new TokenModel();

            if (user == null)
            {
                throw new HexagonException("Пользователь не найден");
            }

            var token = await GenerateToken(user.Id);
            ret.Jwt = new JwtSecurityTokenHandler().WriteToken(token);
            ret.Successed = true;

            ret.User = await GetById(user.Id);

            return ret;
        } 
    
         private async Task<JwtSecurityToken> GenerateToken(Guid userId)
        {
            var userDb = await _userManager.FindByIdAsync(userId.ToString());
            return await GenerateToken(userDb);
        }
        
        private async Task<JwtSecurityToken> GenerateToken(User userDb)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sid, userDb.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userDb.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var roles = await _hexagonContext.UserRoles.Where(x => x.UserId == userDb.Id)
                .Include(z => z.Role)
                .Select(x => x.Role.Name).ToListAsync();
            
            AddRolesToClaims(claims, roles);

            return new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:Key"))),
                    SecurityAlgorithms.HmacSha256),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1)
            );
        }
        
        private void AddRolesToClaims(List<Claim> claims, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
        }
        
        public async Task<GetUserModel> GetById(Guid id)
        {
            //var userRoles = GetUserRoleNames(id);
            var response = await _hexagonContext.Users.Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<GetUserModel>(response);
        }
}