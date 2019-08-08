using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Testing;
using Model.Models;
using Service.Results;
using Service.Services;
using Service.SessionHandlers;
using HttpMethod = System.Net.Http.HttpMethod;

namespace AudioWeb.Controllers
{
  [SessionAuthorize]
  [RoutePrefix("api/account")]
  public class AccountController : ApiController
  {
    private readonly UserService _userService;

    public AccountController()
    {
      this._userService = new UserService();
    }


    private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

    // POST api/Account/Register
    [HttpPost, AllowAnonymous, Route("Register")]
    public async Task<IHttpActionResult> RegisterUser(User model, bool isExternal = false)
    {

      if (model == null)
      {
        return this.BadRequest("Invalid user data.");
      }

      var userPicture =
        "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIj8+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iTGF5ZXJfMSIgeD0iMHB4IiB5PSIwcHgiIHZpZXdCb3g9IjAgMCAyOTkuOTk3IDI5OS45OTciIHN0eWxlPSJlbmFibGUtYmFja2dyb3VuZDpuZXcgMCAwIDI5OS45OTcgMjk5Ljk5NzsiIHhtbDpzcGFjZT0icHJlc2VydmUiIHdpZHRoPSI1MTJweCIgaGVpZ2h0PSI1MTJweCIgY2xhc3M9IiI+PGc+PGc+Cgk8Zz4KCQk8cGF0aCBkPSJNMTQ5Ljk5NiwwQzY3LjE1NywwLDAuMDAxLDY3LjE1OCwwLjAwMSwxNDkuOTk3YzAsODIuODM3LDY3LjE1NiwxNTAsMTQ5Ljk5NSwxNTBzMTUwLTY3LjE2MywxNTAtMTUwICAgIEMyOTkuOTk2LDY3LjE1NiwyMzIuODM1LDAsMTQ5Ljk5NiwweiBNMTUwLjQ1MywyMjAuNzYzdi0wLjAwMmgtMC45MTZIODUuNDY1YzAtNDYuODU2LDQxLjE1Mi00Ni44NDUsNTAuMjg0LTU5LjA5N2wxLjA0NS01LjU4NyAgICBjLTEyLjgzLTYuNTAyLTIxLjg4Ny0yMi4xNzgtMjEuODg3LTQwLjUxMmMwLTI0LjE1NCwxNS43MTItNDMuNzM4LDM1LjA4OS00My43MzhjMTkuMzc3LDAsMzUuMDg5LDE5LjU4NCwzNS4wODksNDMuNzM4ICAgIGMwLDE4LjE3OC04Ljg5NiwzMy43NTYtMjEuNTU1LDQwLjM2MWwxLjE5LDYuMzQ5YzEwLjAxOSwxMS42NTgsNDkuODAyLDEyLjQxOCw0OS44MDIsNTguNDg4SDE1MC40NTN6IiBkYXRhLW9yaWdpbmFsPSIjMDAwMDAwIiBjbGFzcz0iYWN0aXZlLXBhdGgiIGRhdGEtb2xkX2NvbG9yPSIjQzRDNEM1IiBmaWxsPSIjQzhDOENBIi8+Cgk8L2c+CjwvZz48L2c+IDwvc3ZnPgo=";
      if (!this.ModelState.IsValid)
      {
        return this.BadRequest(this.ModelState);
      }

      var emailExists = _userService.CheckIfUserExists(model.Email);
      if (emailExists && !isExternal)
      {
        return this.BadRequest("E-mail is already taken.");
      }

      var isExternalCompleted = false;
      if (isExternal && model.UserExternalLogins.Count > 0)
      {
        isExternalCompleted = true;
        if (model.UserExternalLogins[0].LoginProvider.ToLower() == "facebook")
        {
          userPicture = "https://graph.facebook.com/" + model.UserExternalLogins[0].ProviderKey + "/picture";
        }
      }
      var user = new User()
      {
        Fullname = model.Fullname,
        Email = model.Email,
        Photo = userPicture,
        Password = _userService.HashPassword(model.Password),
        IsExtraLogged = isExternal,
        EmailConfirmed = model.EmailConfirmed
      };

      var isSuccedded = _userService.AddUser(user);

      if (isSuccedded == null)
      {
        return this.BadRequest("Sorry! Something went wrong");
      }

      if (isExternalCompleted)
      {
        model.UserExternalLogins[0].UserId = user.Id;
        _userService.AddUserExtrenalLogin(model.UserExternalLogins[0]);
      }

      var loginResult = await this.LoginUser(new LoginUserViewModel()
      {
        ClientId = user.Id,
        Password = user.Password
      }, true);

      return loginResult;
    }


    // POST api/Account/Login
    [HttpPost, AllowAnonymous, Route("Login")]
    public async Task<IHttpActionResult> LoginUser(LoginUserViewModel model, bool isExternal = false)
    {
      if (!isExternal)
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        if (model == null)
        {
          return this.BadRequest("Invalid user data");
        }

        var user = _userService.GetUserByEmail(model.Email);
        if (user == null) return BadRequest("Incorrect user data");
        if (!_userService.CheckPassword(model.Password, user.Password))
        {
          return BadRequest("Incorrect user data");
        }

        model.ClientId = user.Id;
      }

      // Invoke the "token" OWIN service to perform the login (POST /token)
      // Use Microsoft.Owin.Testing.TestServer to perform in-memory HTTP POST request
      var testServer = TestServer.Create<Startup>();
      var requestParams = new List<KeyValuePair<string, string>>
      {
        new KeyValuePair<string, string>("grant_type", "password"),
        new KeyValuePair<string, string>("username", model.ClientId.ToString()),
        new KeyValuePair<string, string>("password", model.Password)
      };

      var requestContext = HttpContext.Current.Request;
      var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
      testServer.BaseAddress = new Uri("https://" + Request.RequestUri.Authority);
      var request = testServer.CreateRequest("/Token").And(x => x.Method = HttpMethod.Post)
          .And(x => x.Content = requestParamsFormUrlEncoded);
      var tokenServiceResponse = await request.PostAsync();

      if (tokenServiceResponse.StatusCode != HttpStatusCode.OK) return this.ResponseMessage(tokenServiceResponse);
      // Sucessful login --> create user session in the database
      var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
      var jsSerializer = new JavaScriptSerializer();
      var responseData =
        jsSerializer.Deserialize<Dictionary<string, string>>(responseString);
      var authToken = responseData["access_token"];
      var identity = int.Parse(responseData["userName"]);
      var owinContext = this.Request.GetOwinContext();
      var userSessionManager = new UserSessionManager(owinContext, _userService);
      if (identity > 0)
      {
        userSessionManager.CreateUserSession(identity, authToken, requestContext);
      }
      // Cleanup: delete expired sessions from the database
      userSessionManager.DeleteExpiredSessions();

      return this.ResponseMessage(tokenServiceResponse);
    }

    // POST api/Account/Login
    [HttpGet]
    [OverrideAuthentication]
    [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
    [AllowAnonymous]
    [Route("SocialLogin", Name = "SocialLogin")]
    public async Task<IHttpActionResult> SocialLogin(string provider, string error = null)
    {
      if (error != null)
      {
        return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
      }

      if (!User.Identity.IsAuthenticated)
      {
        return new ChallengeResult(provider, this);
      }

      ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
      if (externalLogin == null)
      {
        return InternalServerError();
      }

      if (externalLogin.LoginProvider != provider)
      {
        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        return new ChallengeResult(provider, this);
      }

      var user = _userService.GetUserByProviderData(externalLogin.ProviderKey, externalLogin.LoginProvider);
      var hasRegistered = user != null;

      IEnumerable<Claim> claims = externalLogin.GetClaims();
      var identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
      IHttpActionResult result;
      string url = "", loginType = "&login";
      if (hasRegistered) // if user hasn't registered yet, we should add him to database, so he'll be able to pass all the next logins
      {
        result = await LoginUser(new LoginUserViewModel() { Password = externalLogin.ProviderKey, ClientId = user.Id}, true);
      }
      else
      {
        loginType = "&register_" + externalLogin.LoginProvider.ToLower();
        var userExternalLogin = new UserExternalLogin
        {
          LoginProvider = externalLogin.LoginProvider,
          ProviderKey = externalLogin.ProviderKey
        };
        result = await RegisterUser(new User()
         {
           Fullname = identity.Name,
           Email = externalLogin.Email,
           Password = externalLogin.ProviderKey,
           IsExtraLogged = true,
           UserExternalLogins = new List<UserExternalLogin> { userExternalLogin }
      }, true);
      }

      if (result is BadRequestErrorMessageResult)
      {
        url = "https://" + Request.RequestUri.Authority + "#error_login=" + ((BadRequestErrorMessageResult)result).Message.Replace(" ", "_");
        var uri = new Uri(url);
        return Redirect(uri);
      }

      var userId = _userService.GetUserByProviderData(externalLogin.ProviderKey, externalLogin.LoginProvider).Id;
      var token = _userService.GetUserSession(userId).AuthToken;
      url = "https://" + Request.RequestUri.Authority + "#access_token=" + token + loginType;
      return Redirect(url);
    }

    [HttpGet]
    [Route("CheckToken")]
    public JsonResult<User> CheckToken()
    {
      var user = _userService.GetUserByIdentityName(User.Identity.Name);
      user.Password = null;
      return Json(user);
    }

    [HttpPost]
    [Route("UpdateUser")]
    public JsonResult<bool> UpdateUser(User user)
    {
      if (user == null) return Json(false);
      if (user.Id.ToString() == User.Identity.Name) _userService.UpdateUser(user, false);
      else return Json(false);
      return Json(true);
    }

    [HttpPost]
    [Route("TerminateSession")]
    public JsonResult<bool> TerminateSession([FromBody] int id)
    {
      var userIdentity = User.Identity.Name;
      _userService.TerminateSession(id, int.Parse(userIdentity));
      return Json(true);
    }

    [HttpPost]
    [Route("ChangePassword")]
    public JsonResult<bool> ChangePassword(ChangingPasswordViewModel cpvm)
    {
      if (cpvm == null) return Json(false);
      var user = _userService.GetUserByIdentityName(User.Identity.Name);
      if (cpvm.NewPassword != cpvm.ConfirmPassword) return Json(false);
      if (!_userService.CheckPassword(cpvm.CurrentPassword, user.Password)) return Json(false);
      user.Password = _userService.HashPassword(cpvm.NewPassword);
      _userService.UpdateUser(user, true);
      return Json(true);
    }

    [HttpGet]
    [Route("GetActiveSessions")]
    public async Task<IHttpActionResult> GetActiveSessions()
    {

      var userSessionManager = new UserSessionManager(_userService);
      userSessionManager.DeleteExpiredSessions();
      var user = _userService.GetUserByIdentityName(User.Identity.Name);
      var sessions = await _userService.GetUserSessions(user.Id);
      var result = sessions.OrderByDescending(x => x.ExpirationDateTime)
        .Select(session => new JsonObject
        {
          ["Id"] = session.Id,
          ["IpAddress"] = session.IpAddress,
          ["UserAgent"] = session.UserAgent,
          ["ExpiresIn"] = (session.ExpirationDateTime - DateTime.Now).Days == 0 ? (session.ExpirationDateTime - DateTime.Now).Hours + " hours" : (session.ExpirationDateTime - DateTime.Now).Days + " days"
        })
        .ToList();
      return Json(result);
    }

    // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
    [AllowAnonymous]
    [Route("ExternalLogins")]
    public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
    {
      IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();

      string state;

      if (generateState)
      {
        const int strengthInBits = 256;
        state = RandomOAuthStateGenerator.Generate(strengthInBits);
      }
      else
      {
        state = null;
      }

      return descriptions.Select(description => new ExternalLoginViewModel
      {
        Name = description.Caption,
        Url = Url.Route("SocialLogin", new
        {
          provider = description.AuthenticationType,
          response_type = "token",
          client_id = Startup.PublicClientId,
          redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
          state = state
        }),
        State = state
      })
        .ToList();
    }

    // POST api/Account/Logout
    [HttpPost, Route("Logout")]
    public IHttpActionResult Logout()
    {
      // This does not actually perform logout! The OWIN OAuth implementation
      // does not support "revoke OAuth token" (logout) by design.
      this.Authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);

      // DeleteSongFromInstance the user's session from the database (revoke its bearer token)
      var owinContext = this.Request.GetOwinContext();
      var userSessionManager = new UserSessionManager(owinContext, _userService);
      userSessionManager.InValidateUserSession();
      return this.Ok(new
      {
        message = "Logout successful."
      });
    }

    private class ExternalLoginData
    {
      public string LoginProvider { get; set; }
      public string ProviderKey { get; set; }
      public string UserName { get; set; }
      public string Email { get; set; }

      public IList<Claim> GetClaims()
      {
        IList<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

        if (UserName != null)
        {
          claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
        }

        if (Email != null)
        {
          claims.Add(new Claim(ClaimTypes.Email, Email, null, LoginProvider));
        }

        return claims;
      }

      public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
      {
        Claim providerKeyClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(providerKeyClaim?.Issuer) || string.IsNullOrEmpty(providerKeyClaim.Value))
        {
          return null;
        }
        if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
        {
          return null;
        }
        return new ExternalLoginData
        {
          LoginProvider = providerKeyClaim.Issuer,
          ProviderKey = providerKeyClaim.Value,
          UserName = identity.FindFirstValue(ClaimTypes.Name),
          Email = identity.FindFirstValue(ClaimTypes.Email)
        };
      }
    }
    private static class RandomOAuthStateGenerator
    {
      private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

      public static string Generate(int strengthInBits)
      {
        const int bitsPerByte = 8;

        if (strengthInBits % bitsPerByte != 0)
        {
          throw new ArgumentException("strengthInBits must be evenly divisible by 8.", nameof(strengthInBits));
        }

        int strengthInBytes = strengthInBits / bitsPerByte;

        byte[] data = new byte[strengthInBytes];
        _random.GetBytes(data);
        return HttpServerUtility.UrlTokenEncode(data);
      }
    }
  }

}
