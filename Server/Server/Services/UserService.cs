using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Server.DAL;
using Server.Models;
namespace Server.Services
{
  public class UserService
  {
    private readonly WebPlayerDbContext _db;
    private readonly IDbRepository<User> _dbUser;
    private readonly IDbRepository<UserExternalLogin> _dbUserExtrenalLogin;
    private readonly IDbRepository<UserSession> _dbSession;

    public UserService()
    {
      _db = new WebPlayerDbContext();
      _dbUser = new DbRepository<User>(new DefaultDbFactory());
      _dbUserExtrenalLogin = new DbRepository<UserExternalLogin>(new DefaultDbFactory());
      _dbSession = new DbRepository<UserSession>(new DefaultDbFactory());
    }

    public bool CheckIfUserCredentialExists(string username, string password)
    {
      return _db.Users.Any(x => x.Password == password && x.Username == username);
    }

    public bool CheckIfUserExists(string username)
    {
      return _db.Users.Any(x => x.Username == username);
    }

    public UserExternalLogin CheckIfUserExternalLoginExists(string provider, string key)
    {
      return _db.UserExternalLogins.FirstOrDefault(x => x.LoginProvider == provider && x.ProviderKey == key);
    }

    public string AddUser(User user)
    {
      _dbUser.Add(user);
      return user.Id.ToString();
    }
    public string AddUserExtrenalLogin(UserExternalLogin user)
    {
      _dbUserExtrenalLogin.Add(user);
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

    public User GetUserById(int id)
    {
      return _dbUser.Get(x => x.Id == id);
    }

    public UserSession GetUserSession(int id)
    {
      return _dbSession.Get(x => x.OwnerUserId == id);
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
