using Contacting.Domain.Shared.Persons;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contacting.Application.Commands
{
    public class CreatePersonContactCommand : IRequest<bool>
    {
        public Guid PersonId { get; set; }
        public ContactType ContactType { get; set; }
        public string Content { get; set; }


        public CreatePersonContactCommand()
        {
        }

        public CreatePersonContactCommand(Guid personId, ContactType contactType, string content) : this()
        {
            this.PersonId = personId;
            this.ContactType = contactType;
            this.Content = content;
        }
    }
}
