using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{
  public class Song
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public string Duration { get; set; }
    public string AlbumCover { get; set; }
    public string Album { get; set; }
    public string Year { get; set; }
    public string Genre { get; set; }
    public string Source { get; set; }
    public double Activity { get; set; }
  }
}
