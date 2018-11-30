using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{

  public class LoginUserViewModel
  {
    public string Username { get; set; }
    public string Password { get; set; }
  }

  public class ExternalLoginViewModel
  {
    public string Name { get; set; }

    public string Url { get; set; }

    public string State { get; set; }
  }
}
