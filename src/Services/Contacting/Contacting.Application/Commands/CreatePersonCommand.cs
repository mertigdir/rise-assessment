using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contacting.Application.Commands
{
    public class CreatePersonCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }


        public CreatePersonCommand()
        {
        }

        public CreatePersonCommand(string name, string surname, string company) : this()
        {
            this.Name = name;
            this.Surname = surname;
            this.Company = company;
        }
    }
}
