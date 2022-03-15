using Contacting.Domain.Auctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacting.Dto.Auctions
{
    public record PersonDto
    {
        public PersonDto()
        {
            Contacts = new List<PersonContactDto>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<PersonContactDto> Contacts { get; set; }
    }
}
