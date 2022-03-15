using System;
using System.Collections.Generic;
using System.Text;

namespace Contacting.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class ContactingValidationException : Exception
    {
        public ContactingValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
