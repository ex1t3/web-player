using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using Server.Models;
using Server.Providers;
using Server.Results;
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
    public async Task<IHttpActionResult> RegisterUser(User model, bool isExtrenal = false, string provider = null)
    {
      //if (UserSessionManager.IsAuthenticated)
      //{
      //  return this.BadRequest("User is already logged in.");
      //}

      if (model == null)
      {
        return this.BadRequest("Invalid user data.");
      }

      if (!this.ModelState.IsValid)
      {
        return this.BadRequest(this.ModelState);
      }

      var emailExists = _userService.CheckIfUserExists(model.Username, model.Email);
      if (emailExists)
      {
        return this.BadRequest("Username or E-mail are already taken.");
      }

      var user = new User()
      {
        Username = model.Username,
        Name = model.Name,
        Email = model.Email,
        Password = _userService.HashPassword(model.Password),
        IsExtraLogged = isExtrenal,
        EmailConfirmed = model.EmailConfirmed
      };

      var isSuccedded = _userService.AddUser(user);

      if (isSuccedded == null)
      {
        return this.BadRequest("Sorry! Something went wrong");
      }

      if (model.IsExtraLogged)
      {

        _userService.AddUserExtrenalLogin(new UserExternalLogin()
        {
          LoginProvider = provider,
          UserId = user.Id,
          ProviderKey = user.Password
        });
      }
      var loginResult = await this.LoginUser(new LoginUserViewModel()
      {
        Username = model.Username,
        Password = model.Password
      }, true);

      return loginResult;
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

      var user = _userService.GetUserByName(externalLogin.UserName);
      if (_userService.CheckIfUserExists(externalLogin.UserName, externalLogin.Email))
      {
        if (!_userService.CheckPassword(externalLogin.ProviderKey, user.Password))
        {
          return BadRequest("Incorrect user data.");
        }
      }
      bool hasRegistered = user != null;

      if (hasRegistered) //if user isn't registered yet, we should add him to database, so he'll able to pass all the next logins
      {
        IEnumerable<Claim> claims = externalLogin.GetClaims();
        ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);

        var result = await LoginUser(new LoginUserViewModel() { Password = _userService.HashPassword(identity.Name), Username = identity.Name }, true);
        var url = "";
        if (result is BadRequestErrorMessageResult)
        {
          url = "https://" + Request.RequestUri.Authority + "?error_login=" + ((BadRequestErrorMessageResult)result).Message.Replace(" ", "_");
          Uri uri = new Uri(url);
          return Redirect(uri);
        }
        var token = _userService.GetUserSession(_userService.GetUserByName(identity.Name).Id).AuthToken;
        url = "https://" + Request.RequestUri.Authority + "#access_token=" + token;
        return Redirect(url);
      }
      else
      {
        IEnumerable<Claim> claims = externalLogin.GetClaims();
        ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
        var result = await RegisterUser(new User() { Username = identity.Name, Email = externalLogin.Email, Password = _userService.HashPassword(identity.Name), IsExtraLogged = true}, true, externalLogin.LoginProvider);
        var url = "";
        if (result is BadRequestErrorMessageResult)
        {
          url = "https://" + Request.RequestUri.Authority + "#error_login=" + ((BadRequestErrorMessageResult)result).Message.Replace(" ", "_");
          Uri uri = new Uri(url);
          return Redirect(uri);
        }

        var token = _userService.GetUserSession(_userService.GetUserByName(identity.Name).Id).AuthToken;
        url = "https://" + Request.RequestUri.Authority + "#access_token=" + token;
        return Redirect(url);
      }
    }

    [HttpGet]
    [Route("CheckToken")]
    public bool CheckToken()
    {
      return true;
    }

    // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
    [AllowAnonymous]
    [Route("ExternalLogins")]
    public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
    {
      IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
      List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

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

      foreach (AuthenticationDescription description in descriptions)
      {
        ExternalLoginViewModel login = new ExternalLoginViewModel
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
        };
        logins.Add(login);
      }

      return logins;
    }

    // POST api/Account/Login
    [HttpPost, AllowAnonymous, Route("Login")]
    public async Task<IHttpActionResult> LoginUser(LoginUserViewModel model, bool isExternal = false)
    {
      if (!isExternal)
      {
        //if (User.Identity.Name!=null)
        //{
        //  return this.BadRequest("User is already logged in.");
        //}
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        if (model == null)
        {
          return this.BadRequest("Invalid user data");
        }

        var user = _userService.GetUserByName(model.Username);
        if (user == null) return BadRequest("Incorrect user data");
          if (!_userService.CheckPassword(model.Password, user.Password))
          {
            return BadRequest("Incorrect user data");
          }
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
      testServer.BaseAddress = new Uri("https://" + Request.RequestUri.Authority);
      var request = testServer.CreateRequest("/Token").And(x => x.Method = HttpMethod.Post)
          .And(x => x.Content = requestParamsFormUrlEncoded);
      var tokenServiceResponse = await request.PostAsync();

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
        if (identity == null)
        {
          return null;
        }

        Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

        if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                                     || String.IsNullOrEmpty(providerKeyClaim.Value))
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
          throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
        }

        int strengthInBytes = strengthInBits / bitsPerByte;

        byte[] data = new byte[strengthInBytes];
        _random.GetBytes(data);
        return HttpServerUtility.UrlTokenEncode(data);
      }
    }
  }

}
