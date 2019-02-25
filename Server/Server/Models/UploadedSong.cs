using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{
  public class UploadedSong
  {
    public int Id { get; set; }
    public int SongId { get; set; }
    public int UserId { get; set; }
  }
}
