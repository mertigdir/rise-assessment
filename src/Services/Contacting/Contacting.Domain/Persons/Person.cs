using System;
using System.Collections.Generic;
using System.Linq;
using Contacting.Domain.SeedWork;

namespace Contacting.Domain.Persons
{
    public class Person : Entity<Guid>, IAggregateRoot
    {
        protected Person()
        {
            Contacts = new List<PersonContact>();
        }
        public Person(Guid id, string name, string surname, string company, List<PersonContact> contacts = null) : this()
        {
            this.Id = id;
            this.Name = name;
            this.Surname = surname;
            this.Company = company;

            this.Contacts = contacts;
        }

        public string Name { get; protected set; }
        public string Surname { get; protected set; }
        public string Company { get; protected set; }

        public ICollection<PersonContact> Contacts { get; protected set; }
    }
}
