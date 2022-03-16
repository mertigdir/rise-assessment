using Contacting.Dto.Persons;
using Contacting.Dto.Persons.Inputs;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Reporting.Application.Services
{
    public interface IPersonApiClient 
    {
        [Get("/list")]
        Task<List<PersonDto>> GetPersonsAsync(GetPersonListInput input);

        [Get("/{personId}")]
        Task<PersonDto> GetPersonAsync(int id);

        [Put("/person/")]
        Task<PersonDto> CreatePersonAsync([Body] CreatePersonInput person);

        [Delete("/{personId}")]
        Task<bool> DeletePersonAsync(Guid personId);

        [Delete("/{personId}/person-contact/{personContactId}")]
        Task<bool> DeletePersonContactAsync(Guid personId);

        [Put("/{personId}/person-contact")]
        Task<PersonDto> CreatPersonContactAsync([Body] CreatePersonContactInput person);
    }
}
