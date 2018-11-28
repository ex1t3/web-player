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
        private readonly IDbRepository<UserSession> _dbSession; 

        public UserService()
        {
            _db = new WebPlayerDbContext();
            _dbUser = new DbRepository<User>(new DefaultDbFactory());
            _dbSession = new DbRepository<UserSession>(new DefaultDbFactory());
        }

        public User CheckIfUserExist(string username, string password)
        {
            return _db.Users.FirstOrDefault(x => x.Password == password && x.Username == username);
        }

        public string AddUser(User user)
        {
            _dbUser.Add(user);
          return user.Id.ToString();
        }

        public List<User> GetAll()
        {
          return _dbUser.GetAll().ToList();
        }

      public User GetUserByName(string name)
      {
        return _dbUser.Get(x => x.Username == name);
      }

      public void AddSession(UserSession userSession)
      {
        // Extend the lifetime of the current user's session: current moment + fixed timeout
        userSession.ExpirationDateTime = DateTime.Now + new TimeSpan(0, 0, 30, 0);
      _dbSession.Add(userSession);
      }

      public void DeleteSession(UserSession userSession)
      {
        _dbSession.Delete(userSession);
      }
    }
}
