using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Server.Models;
using Microsoft.Owin.Security.OAuth;
using Server.Providers;

namespace Server
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
          Provider = new AppOAuthProvider(PublicClientId),
          AccessTokenExpireTimeSpan = TimeSpan.FromDays(365),
          AllowInsecureHttp = true //Remove after development mode
        };
      }

      public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

      public void ConfigureAuth(IAppBuilder app)
      {
        // Enable CORS
        app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

        // Enable the application to use bearer tokens to authenticate users
        app.UseOAuthBearerTokens(OAuthOptions);
      }

    // Uncomment the following lines to enable logging in with third party login providers  
    //app.UseMicrosoftAccountAuthentication(  
    //    clientId: "",  
    //    clientSecret: "");  

    //app.UseTwitterAuthentication(  
    //   consumerKey: "",  
    //   consumerSecret: "");  

    //app.UseFacebookAuthentication(  
    //   appId: "",  
    //   appSecret: "");  

    //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()  
    //{  
    //    ClientId = "",  
    //    ClientSecret = ""  
    //});  
  }
 }
