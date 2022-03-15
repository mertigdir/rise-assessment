using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reporting.API.Infrastructure.Middlewares
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class NoTransactionAttribute : Attribute
    {
    }
}
