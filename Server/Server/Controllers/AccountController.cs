using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Testing;
using Server.Models;
using Server.Services;
using Server.SessionHandlers;

namespace Server.Controllers
{
  [SessionAuthorize]
  [RoutePrefix("api/account")]
  public class UsersController : ApiController
  {
    private readonly UserService _userService;

    public UsersController()
    {
      this._userService = new UserService();
    }


    private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

    // POST api/Account/Register
    [HttpPost, AllowAnonymous, Route("Register")]
    public async Task<IHttpActionResult> RegisterUser(User model)
    {
      if (UserSessionManager.IsAuthenticated)
      {
        return this.BadRequest("User is already logged in.");
      }

      if (model == null)
      {
        return this.BadRequest("Invalid user data.");
      }

      if (!this.ModelState.IsValid)
      {
        return this.BadRequest(this.ModelState);
      }

      var emailExists = _userService.GetAll().Any(x => x.Email == model.Email);
      if (emailExists)
      {
        return this.BadRequest("Email is already taken.");
      }

      var user = new User()
      {
        Username = model.Username,
        Name = model.Name,
        Email = model.Email,
      };

      var isSuccedded = _userService.AddUser(user);

      if (isSuccedded!=null)
      {
        return this.BadRequest("Sorry! Something went wrong");
      }

      var loginResult = await this.LoginUser(new LoginUserViewModel()
      {
        Username = model.Username,
        Password = model.Password
      });

      return loginResult;
    }

    // POST api/Account/Login
    [HttpPost, AllowAnonymous, Route("Login")]
    public async Task<IHttpActionResult> LoginUser(LoginUserViewModel model)
    {
      if (UserSessionManager.IsAuthenticated)
      {
        return this.BadRequest("User is already logged in.");
      }
      if (model == null)
      {
        return this.BadRequest("Invalid user data");
      }
      // Invoke the "token" OWIN service to perform the login (POST /token)
      // Use Microsoft.Owin.Testing.TestServer to perform in-memory HTTP POST request
      var testServer = TestServer.Create<Startup>();
      var requestParams = new List<KeyValuePair<string, string>>
      {
        new KeyValuePair<string, string>("grant_type", "password"),
        new KeyValuePair<string, string>("username", model.Username),
        new KeyValuePair<string, string>("password", model.Password)
      };

      var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
      var tokenServiceResponse = await testServer.HttpClient.PostAsync(
        "/Token", requestParamsFormUrlEncoded);

      if (tokenServiceResponse.StatusCode == HttpStatusCode.OK)
      {
        // Sucessful login --> create user session in the database
        var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
        var jsSerializer = new JavaScriptSerializer();
        var responseData =
          jsSerializer.Deserialize<Dictionary<string, string>>(responseString);
        var authToken = responseData["access_token"];
        var username = responseData["userName"];
        var owinContext = this.Request.GetOwinContext();
        var userSessionManager = new UserSessionManager(owinContext, _userService);
        userSessionManager.CreateUserSession(username, authToken);

        // Cleanup: delete expired sessions from the database
        userSessionManager.DeleteExpiredSessions();
      }

      return this.ResponseMessage(tokenServiceResponse);
    }

    // POST api/Account/Logout
    [HttpPost, Route("Logout")]
    public IHttpActionResult Logout()
    {
      // This does not actually perform logout! The OWIN OAuth implementation
      // does not support "revoke OAuth token" (logout) by design.
      this.Authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);

      // Delete the user's session from the database (revoke its bearer token)
      var owinContext = this.Request.GetOwinContext();
      var userSessionManager = new UserSessionManager(owinContext, _userService);
      userSessionManager.InValidateUserSession();
      return this.Ok(new
      {
        message = "Logout successful."
      });
    }
  }
}
