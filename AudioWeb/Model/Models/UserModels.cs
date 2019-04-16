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
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    public bool IsExtraLogged { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public IEnumerable<UserExternalLogin> UserExternalLogins { get; set; }
    public IEnumerable<UserSession> UserSessions { get; set; }

    public static int LastPlayedSongLimit = 5;
  }

  public class UserSession
  {
    public int Id { get; set; }
    public int OwnerUserId { get; set; }
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
}
