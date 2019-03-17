using DAL;
using Service.Services;

namespace Service.SessionHandlers
{
  using System.Linq;
  using System.Net;
  using System.Net.Http;
  using System.Web.Http;
  using System.Web.Http.Controllers;

  public class SessionAuthorizeAttribute : AuthorizeAttribute
  {
    protected readonly UserService _userService;
    public SessionAuthorizeAttribute() : this(new UserService())
    {
    }
    public SessionAuthorizeAttribute(UserService us)
    {
      _userService = us;
    }


    public override void OnAuthorization(HttpActionContext actionContext)
    {
      if (SkipAuthorization(actionContext))
      {
        return;
      }

      var requestProperties = actionContext.Request.GetOwinContext();
      var userSessionManager = new UserSessionManager(requestProperties, _userService);
      if (userSessionManager.ReValidateSession())
      {
        base.OnAuthorization(actionContext);
      }
      else
      {
        actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(
          HttpStatusCode.Unauthorized, "Session token expired or not valid.");
        UserSessionManager.IsAuthenticated = false;
      }
    }

    private static bool SkipAuthorization(HttpActionContext actionContext)
    {
      return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
             || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
    }
  }
}
