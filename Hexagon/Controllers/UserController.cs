using Hexagon.Logic.Logic;
using Hexagon.Logic.Models;
using Hexagon.Logic.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hexagon.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : BaseController<IUserLogic, UserController>
{
    public UserController(ILogger<UserController> logger, IUserLogic logic, IHttpContextAccessor context): 
        base(logic, context, logger) { }

    [HttpPost("sign-up")]
    [AllowAnonymous]
    public async Task<ActionResult<Guid>> SignUp([FromBody] SignUpRequestModel model)
    {
        return await _logic.SignUp(model);
    }

    [HttpGet("sign-in")]
    [AllowAnonymous]
    public async Task<TokenModel> SignIn([FromQuery] SignInRequestModel model)
    {
        return await _logic.SignInByLogin(model);
    }  
}
