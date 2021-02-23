using System.Collections.Generic;

namespace Payments.Application.Common.Models
{
    public class PaginationResponse<T>
    {
        public List<T> Items { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }
    }
}
