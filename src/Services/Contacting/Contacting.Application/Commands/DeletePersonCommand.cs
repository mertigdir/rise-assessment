using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contacting.Application.Commands
{
    public class DeletePersonCommand : IRequest<bool>
    {
        public Guid PersonId { get; set; }


        public DeletePersonCommand()
        {
        }

        public DeletePersonCommand(Guid personId) : this()
        {
            this.PersonId = personId;
        }
    }
}
