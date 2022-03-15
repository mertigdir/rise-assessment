using Contacting.Application.IntegrationEvents.Events;
using Contacting.Domain.Auctions;
using Contacting.Dto.Auctions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Contacting.Domain.Persons;

namespace Contacting.Application.Mapper.Persons
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonDto>();
            CreateMap<PersonContact, PersonContactDto>();
        }
    }
}
