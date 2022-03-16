using System;
using System.Collections.Generic;
using System.Linq;
using Contacting.Domain.Shared.Persons;

namespace Contacting.Dto.Persons
{
    public record PersonContactDto
    {
        public Guid Id { get; set; }
        public ContactType ContactType { get; set; }
        public string Content { get; set; }
    }
}
