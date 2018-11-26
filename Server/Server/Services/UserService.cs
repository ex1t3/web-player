using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Server.DAL;
using Server.Models;
namespace Server.Services
{
    public class UserService
    {
        private readonly WebPlayerDbContext _db;
        private readonly IDbRepository<User> _dbUser; 

        public UserService()
        {
            _db = new WebPlayerDbContext();
            _dbUser = new DbRepository<User>(new DefaultDbFactory());
        }

        public User CheckIfUserExist(string username, string password)
        {
            return _db.Users.FirstOrDefault(x => x.Password == password && x.Username == username);
        }

        public void AddUser(User user)
        {
            _dbUser.Add(user);
        }
    }
}