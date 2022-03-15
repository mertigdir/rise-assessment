using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Paginations
{
    public class PagedInputDto
    {
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }

        public PagedInputDto()
        {
            MaxResultCount = 1000;
        }
    }
}
