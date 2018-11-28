using System;

namespace Server.Models
{
  public class UserSession
  {
    public int Id { get; set; }
    public string OwnerUserId { get; set; }
    public string AuthToken { get; set; }
    public DateTime ExpirationDateTime { get; set; }
  }
}
