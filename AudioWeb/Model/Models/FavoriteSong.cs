using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Models
{
  public class FavoriteSong
  {
    public int Id { get; set; }
    public int SongId { get; set; }
    public int UserId { get; set; }
  }
}
