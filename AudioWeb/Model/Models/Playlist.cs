using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Models
{
  public class Playlist
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Cover { get; set; }
    public int UserId { get; set; }
    public DateTime Created { get; set; }
  }
}
