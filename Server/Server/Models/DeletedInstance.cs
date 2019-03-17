using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{
  public class DeletedInstance
  {
    public int SongId { get; set; }
    public int Type { get; set; }
    public int PlaylistId { get; set; }
  }
}
