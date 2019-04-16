using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DAL;
using DAL.Repository;
using DevOne.Security.Cryptography.BCrypt;
using Model.Models;
namespace Service.Services
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
      return _db.Users.Any(x => x.Password == password && x.Username == username && x.IsExtraLogged == false);
    }

    public bool CheckIfUserExists(string username, string email)
    {
      return _db.Users.Any(x => x.Username == username || x.Email == email && email != null);
    }

    public User GetUserByProviderKey(string providerKey, string loginProvider)
    {
      var userExternalLogin =
        _dbUserExtrenalLogin.Get(x => x.ProviderKey == providerKey && x.LoginProvider == loginProvider);
      return userExternalLogin != null ? GetUserById(userExternalLogin.UserId) : null;
    }

    public string HashPassword(string password)
    {
      return BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt(10));
    }

    public bool CheckPassword(string password, string hashed)
    {
      return BCryptHelper.CheckPassword(password, hashed);
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

    public UserSession GetUserSession(int userId)
    {
      return _db.UserSessions.OrderByDescending(x => x.ExpirationDateTime).FirstOrDefault(x => x.OwnerUserId == userId);
    }

    public void AddSession(UserSession userSession, TimeSpan defaultTimeOut)
    {
      // Extend the lifetime of the current user's session: current moment + fixed timeout
      userSession.ExpirationDateTime = DateTime.Now + defaultTimeOut;
      _dbSession.Add(userSession);
    }

    public void DeleteSession(UserSession userSession)
    {
      _dbSession.Delete(userSession);
    }
  }
}
