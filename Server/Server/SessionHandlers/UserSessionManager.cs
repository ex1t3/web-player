using System.Collections.Generic;
using System.Web.Http.Controllers;
using Server.DAL;
using Server.Services;
namespace Server.SessionHandlers
{
  using System;
  using System.Linq;
  using System.Net.Http;
  using System.Web;
  using Microsoft.AspNet.Identity;

  using Server.Models;
  using Microsoft.Owin;

  public class UserSessionManager
  {
    private static readonly TimeSpan DefaultSessionTimeout = new TimeSpan(0, 0, 30, 0);
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
      if (this.OwinContext.Authentication.User.Identity == null)
      {
        return null;
      }

      return this.OwinContext.Authentication.User.Identity.GetUserId();
    }

    /// <summary>
    /// Extends the validity period of the current user's session in the database.
    /// This will configure the user's bearer authorization token to expire after
    /// certain period of time (e.g. 30 minutes, see UserSessionTimeout in Web.config)
    /// </summary>
    public void CreateUserSession(string username, string authToken)
    {
      var userId = _userService.GetUserByName(username).Id;
      IsAuthenticated = true;
      var userSession = new UserSession()
      {
        OwnerUserId = userId.ToString(),
        AuthToken = authToken
      };
      _userService.AddSession(userSession);
      
    }

    /// <summary>
    /// Makes the current user session invalid (deletes the session token from the user sessions).
    /// The goal is to revoke any further access with the same authorization bearer token.
    /// Typically this method is called at "logout".
    /// </summary>
    public void InValidateUserSession()
    {
      string authToken = this.GetCurrentBearerAuthrorizationToken();
      authToken = authToken?.Substring(7);
      var currentUserId = this.GetCurrentUserId();
      var userSession = this._db.UserSessions.FirstOrDefault(session =>
          session.AuthToken == authToken && session.OwnerUserId == currentUserId);
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
      string authToken = this.GetCurrentBearerAuthrorizationToken();
      authToken = authToken?.Substring(7);
      IsAuthenticated = true;
      var currentUserId = this.GetCurrentUserId();
      var userSession = this._db.UserSessions.FirstOrDefault(session => session.AuthToken == authToken && session.OwnerUserId == currentUserId);

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
