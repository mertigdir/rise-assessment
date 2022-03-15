using Contacting.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Contacting.Domain.SeedWork;
using Contacting.Domain.Events;
using Contacting.Domain.Shared.Persons;

namespace Contacting.Domain.Persons
{
    public class PersonContact : Entity<Guid>
    {

        protected PersonContact()
        {
        }
        public PersonContact(Guid id, Guid personId, ContactType contactType, string content) : this()
        {
            this.Id = id;
            this.ContactType = contactType;
            this.Content = content;
            this.PersonId = personId;
        }

        public ContactType ContactType { get; protected set; }
        public string Content { get; protected set; }
        public virtual Guid PersonId { get; protected set; }

    }
}
