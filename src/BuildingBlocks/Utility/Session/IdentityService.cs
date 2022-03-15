using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility.Session
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Guid UserId => GetUserIdentity();

        public Guid GetUserIdentity()
        {
            return  new Guid(_context.HttpContext.User.FindFirst("sub")?.Value);
        }

        public string GetUserName()
        {
            return _context.HttpContext.User.Identity?.Name;
        }
    }
}
