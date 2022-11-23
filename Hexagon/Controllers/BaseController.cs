using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Globalization;
using Hexagon.Dtos;
using Hexagon.Logic.Logic;
using Microsoft.AspNetCore.Http;

namespace Hexagon.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
//[Authorize] // TODO think
public class BaseController<TService, TController> : Controller where TService : IBaseLogic where TController : Controller
{
    private int _userId = 0;

    // ReSharper disable once InconsistentNaming
    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly TService _logic;
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly ILogger<TController> _logger;

    public BaseController(TService service, IHttpContextAccessor context, ILogger<TController> logger)
    {
        _logger = logger;

        _logic = service;
        var user = context.HttpContext?.User;
        if (user != null && user.Claims.Any())
        {
#pragma warning disable CS8602
            _userId = Convert.ToInt32(user.Identities.FirstOrDefault()
#pragma warning restore CS8602
                .Claims.FirstOrDefault(x => x.Type == "sid")?
                .Value);
        }

        _logic.UserId = _userId;
    }

    protected int CurrentUserId
    {
        get
        {
            if (_userId != 0) return _userId;
            var userIdentityId = User.Claims.FirstOrDefault()?.Value;
            
            var claimValue = User
                .Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?
                .Value;
            if (userIdentityId != null) _userId = int.Parse(userIdentityId);

            return _userId;
        }
    }

#pragma warning disable CS8625
    protected IActionResult ExceptionResult(Exception ex, object args = null)
#pragma warning restore CS8625
    {
        var controllerName = ControllerContext.ActionDescriptor.ControllerName;
        var actionName = ControllerContext.ActionDescriptor.ActionName;
        var msg = $"{controllerName} {actionName} {ex.Message}";

        var res = ApiResult.Error(ex);

        if (ex is ArgumentException)
        {
            _logger.LogWarning(message: msg, args: args); // TODO ex ?
            return BadRequest(res);
        }

        _logger.LogWarning(message: msg, args: args); // TODO ex ?
        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return Json(res);
    }

    [NonAction]
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(ApiResult.Error(context.ModelState));
        }
    }
}