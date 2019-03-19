using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
  public class LastPlayedSong
  {
    public int Id { get; set; }
    public int SongId { get; set; }
    public int UserId { get; set; }
    public DateTime PlayedDateTime { get; set; }
  }
}
