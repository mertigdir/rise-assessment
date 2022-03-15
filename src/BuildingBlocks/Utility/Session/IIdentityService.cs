using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.Dependency;

namespace Utility.Session
{
    public interface IIdentityService : ITransientDependency
    {
        Guid UserId { get; }

        Guid GetUserIdentity();

        string GetUserName();
    }
}
