using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Server.Models
{
    public class User
    {
      public int Id { get; set; }
      public string Username { get; set; }
      public string Password { get; set; }
      public string Name { get; set; }
      public string Email { get; set; }
  }
}
