using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Server.Models
{

  public class LoginUserViewModel
  {
    [Required]
    public string Username { get; set; }
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
