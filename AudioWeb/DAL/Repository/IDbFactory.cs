using System;

namespace DAL.Repository
{
    // Factory for getting Db instance.
    // Looks useless, but it seems to be a good practice -
    // You are guaranteed to have only one unique instance 
    // for all users in any period in time.
    public interface IDbFactory : IDisposable
    {
        WebPlayerDbContext Get();
    }
}
