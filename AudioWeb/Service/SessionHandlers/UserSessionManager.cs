using System;
using System.Linq;
using System.Web;
using DAL;
using Microsoft.Owin;
using Model.Models;
using Service.Services;
using Microsoft.AspNet.Identity;

namespace Service.SessionHandlers
{
  public class UserSessionManager
  {
    private static readonly TimeSpan DefaultSessionTimeout = new TimeSpan(30, 0, 0, 0);
    public static bool IsAuthenticated { get; set; }

    protected readonly IOwinContext OwinContext;
    protected readonly WebPlayerDbContext _db;
    private readonly UserService _userService;

    public UserSessionManager(IOwinContext owinContext, UserService userService)
    {
      OwinContext = owinContext;
      _db = new WebPlayerDbContext();
      _userService = userService;
    }

    public UserSessionManager(UserService userService)
    {
      _userService = userService;
      _db = new WebPlayerDbContext();
    }

    /// <returns>The current bearer authorization token from the HTTP headers</returns>
    private string GetCurrentBearerAuthrorizationToken()
    {
      string authToken = null;
      if (this.OwinContext.Request.Headers["Authorization"] != null)
      {
        authToken = this.OwinContext.Request.Headers["Authorization"];
      }

      return authToken;
    }

    private string GetCurrentUserId()
    {
      return OwinContext.Authentication.User.Identity?.GetUserId();
    }

    /// <summary>
    /// Extends the validity period of the current user's session in the database.
    /// This will configure the user's bearer authorization token to expire after
    /// certain period of time (e.g. 30 minutes, see UserSessionTimeout in Web.config)
    /// </summary>
    public void CreateUserSession(int userId, string authToken, HttpRequest request)
    {
      IsAuthenticated = true;
      var userSession = new UserSession()
      {
        UserId = userId,
        AuthToken = authToken,
        UserAgent = request.UserAgent,
        IpAddress = request.UserHostAddress
      };
      _userService.AddSession(userSession, DefaultSessionTimeout);
      
    }

    /// <summary>
    /// Makes the current user session invalid (deletes the session token from the user sessions).
    /// The goal is to revoke any further access with the same authorization bearer token.
    /// Typically this method is called at "logout".
    /// </summary>
    public void InValidateUserSession()
    {
      var authToken = this.GetCurrentBearerAuthrorizationToken();
      authToken = authToken?.Substring(7);
      var currentUserId = int.Parse(this.GetCurrentUserId());
      var userSession = this._db.UserSessions.FirstOrDefault(session =>
          session.AuthToken == authToken && session.UserId == currentUserId);
      IsAuthenticated = false;
      if (userSession != null)
      {
        _userService.DeleteSession(userSession);
      }
    }

    /// <summary>
    /// Re-validates the user session. Usually called at each authorization request.
    /// If the session is not expired, extends it lifetime and returns true.
    /// If the session is expired or does not exist, return false.
    /// </summary>
    /// <returns>true if the session is valid</returns>
    public bool ReValidateSession()
    {
      var authToken = this.GetCurrentBearerAuthrorizationToken();
      authToken = authToken?.Substring(7);
      IsAuthenticated = true;
      var currentUserId = this.GetCurrentUserId() == null ? 0 : int.Parse(GetCurrentUserId());
      var userSession = this._db.UserSessions.FirstOrDefault(session => session.AuthToken == authToken && session.UserId == currentUserId);

      if (userSession == null)
      {
        // User does not have a session with this token --> invalid session
        return false;
      }

      if (userSession.ExpirationDateTime < DateTime.Now)
      {
        // User's session is expired --> invalid session
        return false;
      }

      // Extend the lifetime of the current user's session: current moment + fixed timeout
      userSession.ExpirationDateTime = DateTime.Now + DefaultSessionTimeout;
      this._db.SaveChanges();

      return true;
    }

    public void DeleteExpiredSessions()
    {
      foreach (var item in _db.UserSessions.Where(session => session.ExpirationDateTime < DateTime.Now))
      {
        _userService.DeleteSession(item);
      }
    }
  }
}
