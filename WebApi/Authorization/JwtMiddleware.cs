namespace WebApi.Authorization;

using Microsoft.Extensions.Options;
using WebApi.Helpers;
using WebApi.Services;
using WebApi.Entities;
using WebApi.Models.Users;
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            if(await userService.CheckTokenLoggedout(token))
            {
                context.Response.StatusCode = 401;
                return;
            }
            var userIdExp = jwtUtils.ValidateJwtToken(token);
            if (userIdExp != null)
            {
                // attach user to context on successful jwt validation
                var arr = userIdExp.Split("  ");
                User user = await userService.GetById(Convert.ToInt32(arr[0]));
                DateTime exp = Convert.ToDateTime(arr[1]);
                
                if(user.IsDisabled)
                {
                    context.Response.StatusCode = 401;
                    return;
                }
                //if (user != null && exp != null)
                if (user != null)
                {
                    context.Items["UserTokenInfo"] = new MiddlewareInfo
                    {
                        User = user,
                        Exp = exp,
                        Token = token
                    };
                }

            }
        }


        await _next(context);
    }
}