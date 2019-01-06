using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{
  public class PlaylistSongs
  {
    public int Id { get; set; }
    public int PlaylistId { get; set; }
    public int SongId { get; set; }
  }
}
