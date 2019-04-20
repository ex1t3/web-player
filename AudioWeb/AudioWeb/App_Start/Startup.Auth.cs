using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Model.Models;
using Microsoft.Owin.Security.OAuth;
using Owin.Security.Providers.Facebook;
using Service.Providers;

namespace AudioWeb
{
  public partial class Startup
  {
    #region Public /Protected Properties.  

    /// <summary>  
    /// OAUTH options property.  
    /// </summary> 

    /// <summary>  
    /// Public client ID property.  
    /// </summary>  
    public const string PublicClientId = "self";

    #endregion

    static Startup()
    {
      OAuthOptions = new OAuthAuthorizationServerOptions
      {
        TokenEndpointPath = new PathString("/Token"),
        AuthorizeEndpointPath = new PathString("/api/Account/SocialLogin"),
        Provider = new AppOAuthProvider(PublicClientId),
        AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
        AllowInsecureHttp = true // Remove after development mode
      };
    }

    public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

    public void ConfigureAuth(IAppBuilder app)
    {
      // Enable CORS
      app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

      // Enable the application to use bearer tokens to authenticate users
      app.UseOAuthBearerTokens(OAuthOptions);
      app.UseCookieAuthentication(new CookieAuthenticationOptions());
      app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
      app.UseFacebookAuthentication(new FacebookAuthenticationOptions()
        {
          AppId = "204022623821507",
          AppSecret = "ae7d99936c42f782207d3eb868d5e5b2"
        });
      app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
      {
        ClientId = "598694976413-t3qat6cf9r4vh7gh24tvt7ebn66f5vu7.apps.googleusercontent.com",
        ClientSecret = "RNcndUXj9DpwR_oMY-qBXcJm"
      });
    }
  }
}
