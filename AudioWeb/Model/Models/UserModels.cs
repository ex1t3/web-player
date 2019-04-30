using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Model.Models
{
  public class User
  {   
    public int Id { get; set; }
    [Required]
    public string Fullname { get; set; }
    [Required]
    public string Password { get; set; }
    public bool IsExtraLogged { get; set; }
    public string Email { get; set; }
    public string Photo { get; set; }
    public string Country { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool IsMale { get; set; } = true;
    public List<UserExternalLogin> UserExternalLogins { get; set; }
    public List<UserSession> UserSessions { get; set; }

    public static int LastPlayedSongLimit = 5;
  }

  public class UserSession
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string AuthToken { get; set; }
    public DateTime ExpirationDateTime { get; set; }
    public string UserAgent { get; set; }
    public string IpAddress { get; set; }
  }

  public class UserExternalLogin 
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string LoginProvider { get; set; }
    public string ProviderKey { get; set; }
  }
  public class LoginUserViewModel
  {
    [Required]
    public string Email { get; set; }
    public int ClientId { get; set; }
    [Required]
    public string Password { get; set; }
  }

  public class ExternalLoginViewModel
  {
    public string Name { get; set; }

    public string Url { get; set; }

    public string State { get; set; }
  }
}
