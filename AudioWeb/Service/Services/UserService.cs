using System;
using System.Collections.Generic;
using System.Data.Entity;
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

    public bool CheckIfUserExists(string email)
    {
      return _db.Users.Any(x=> x.Email == email && email != null);
    }

    public User GetUserByProviderData(string providerKey, string loginProvider)
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

    public void UpdateUser(User user, bool isPasswordChanging = false)
    {
      if (!isPasswordChanging)
      {
        var userBeforeUpdate = _db.Users.AsNoTracking().FirstOrDefault(x => x.Id == user.Id);
        if (userBeforeUpdate == null) return;
        user.Password = userBeforeUpdate.Password;
      }
      _dbUser.Update(user);
    }

    public User GetUserByEmail(string email)
    {
      return _dbUser.Get(x => x.Email == email);
    }

    public User GetUserByIdentityName(string name)
    {
      var id = int.Parse(name);
      return _dbUser.GetById(id);
    }

    public async Task<List<UserSession>> GetUserSessions(int id)
    {
      return _dbSession.GetMany(x => x.UserId == id).ToList();
    }

    public User GetUserById(int id)
    {
      return _dbUser.GetById(id);
    }

    public UserSession GetUserSession(int userId)
    {
      return _db.UserSessions.OrderByDescending(x => x.ExpirationDateTime).FirstOrDefault(x => x.UserId == userId);
    }

    public void AddSession(UserSession userSession, TimeSpan defaultTimeOut)
    {
      // Extend the lifetime of the current user's session: current moment + fixed timeout
      userSession.ExpirationDateTime = DateTime.Now + defaultTimeOut;
      _dbSession.Add(userSession);
    }
    public void TerminateSession(int sessionId, int userId)
    {
      var userSession = _dbSession.Get(x => x.Id == sessionId && x.UserId == userId);
      if (userSession != null)
      {
        _dbSession.Delete(userSession);
      }
    }

    public void DeleteSession(UserSession userSession)
    {
      _dbSession.Delete(userSession);
    }
  }
}
