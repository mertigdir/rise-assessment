using Contacting.Domain.Shared.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contacting.Dto.Persons.Inputs
{
    public class CreatePersonContactInput
    {
        public ContactType ContactType { get; set; }
        public string Content { get; set; }
    }
}
