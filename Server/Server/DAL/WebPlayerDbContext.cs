using System.Data.Entity;
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
    }
}