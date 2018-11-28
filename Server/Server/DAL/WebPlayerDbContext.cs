using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Server.Models;

namespace Server.DAL
{
    public class WebPlayerDbContext : DbContext
    {
        public WebPlayerDbContext () : base("name=WebPlayerDbContext")
        {
            //
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
    }
  }
}
