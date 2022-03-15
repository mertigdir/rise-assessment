using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Paginations
{
    public class PaginationResult<T>
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}
