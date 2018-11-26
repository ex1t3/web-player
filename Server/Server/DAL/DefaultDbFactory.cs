using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;using Server.DAL;

namespace Server.DAL
{
    public class DefaultDbFactory : IDbFactory
    {
        private WebPlayerDbContext _db;

        public WebPlayerDbContext Get()
        {
            return _db ?? (_db = new WebPlayerDbContext());
        }

        private bool isDisposed;

        ~DefaultDbFactory()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                DisposeCore();
            }

            isDisposed = true;
        }

        protected void DisposeCore()
        {
            if (_db != null)
                _db.Dispose();
        }
    }
}