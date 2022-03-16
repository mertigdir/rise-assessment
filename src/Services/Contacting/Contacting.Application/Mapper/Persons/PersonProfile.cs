using AutoMapper;
using Contacting.Domain.Persons;
using Contacting.Dto.Persons;

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
