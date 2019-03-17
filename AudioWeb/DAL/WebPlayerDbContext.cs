using System.Data.Entity;
using Model.Models;

namespace DAL
{
  public class WebPlayerDbContext : DbContext
  {
    public WebPlayerDbContext() : base("name=WebPlayerDbContext")
    {
      //
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<UploadedSong> UploadedSongs { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistSong> PlaylistSongs { get; set; }
    public DbSet<FavoriteSong> FavoriteSongs { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<UserExternalLogin> UserExternalLogins { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
    }
  }
}
