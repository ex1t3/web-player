using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Server.Models;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Server.DAL;

namespace Server.Providers
{
  public class AppOAuthProvider : OAuthAuthorizationServerProvider
  {
    private readonly string _publicClientId;

    public AppOAuthProvider(string publicClientId)
    {
      if (publicClientId == null)
      {
        throw new ArgumentNullException("publicClientId");
      }

      _publicClientId = publicClientId;
    }

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
      var userService = new UserService();

      var user = userService.CheckIfUserExists(context.UserName, context.Password);

      if (user == null)
      {
        context.SetError("invalid_grant", "The user name or password is incorrect.");
        return;
      }

      ClaimsIdentity oAuthIdentity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);
      ClaimsIdentity cookiesIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationType);

      oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
      cookiesIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
      oAuthIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
      cookiesIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

      AuthenticationProperties properties = CreateProperties(user.Username);
      AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
      context.Validated(ticket);
      context.Request.Context.Authentication.SignIn(cookiesIdentity);
    }

    public override Task TokenEndpoint(OAuthTokenEndpointContext context)
    {
      foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
      {
        context.AdditionalResponseParameters.Add(property.Key, property.Value);
      }

      return Task.FromResult<object>(null);
    }

    public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
      // Resource owner password credentials does not provide a client ID.
      if (context.ClientId == null)
      {
        context.Validated();
      }

      return Task.FromResult<object>(null);
    }

    public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
    {
      if (context.ClientId == _publicClientId)
      {
        Uri expectedRootUri = new Uri(context.Request.Uri, "/");

        if (expectedRootUri.AbsoluteUri == context.RedirectUri)
        {
          context.Validated();
        }
      }

      return Task.FromResult<object>(null);
    }

    public static AuthenticationProperties CreateProperties(string userName)
    {
      IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };

      return new AuthenticationProperties(data);
    }
  }
}
