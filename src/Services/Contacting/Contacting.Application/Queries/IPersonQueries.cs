using Contacting.Dto.Auctions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utility.Paginations;

namespace Contacting.Application.Queries
{
    public interface IPersonQueries
    {
        Task<PaginationResult<PersonDto>> GetPersonsAsync(
             Guid? id = null,
             string name = null,
             string surname = null,
             string company = null,
             int skip = 1,
             int take = 10
             );
        Task<PersonDto> GetPersonWithContactsAsync(Guid id);

    }
}
