using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contacting.Dto.Persons.Inputs
{
    public class CreatePersonInput
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }


        public CreatePersonInput(string name, string surname, string company) 
        {
            this.Name = name;
            this.Surname = surname;
            this.Company = company;
        }
    }
}
