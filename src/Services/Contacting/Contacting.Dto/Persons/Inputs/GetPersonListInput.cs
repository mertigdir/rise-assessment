using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Paginations;

namespace Contacting.Dto.Persons.Inputs
{
    public class GetPersonListInput : PagedInputDto
    {
        public GetPersonListInput()
        {
            this.MaxResultCount = 10;
            this.SkipCount = 0;
        }
    }
}
