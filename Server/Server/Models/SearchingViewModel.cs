using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{
  public class SearchingViewModel
  {
    public List<Song> Songs { get; set; }
    public string[] Artists { get; set; }
  }
}
