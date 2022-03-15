using System;
using System.Collections.Generic;
using System.Text;

namespace Contacting.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class ContactingDomainException : Exception
    {
        public ContactingDomainException()
        { }

        public ContactingDomainException(string code)
            : base(code)
        { }

        public ContactingDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
