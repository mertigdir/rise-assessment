using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contacting.Application.Commands
{
    public class DeletePersonContactCommand : IRequest<bool>
    {
        public Guid PersonId { get; set; }
        public Guid PersonContactId { get; set; }


        public DeletePersonContactCommand()
        {
        }

        public DeletePersonContactCommand(Guid personId, Guid personContactId) : this()
        {
            this.PersonId = personId;
            this.PersonContactId = personContactId;
        }
    }
}
