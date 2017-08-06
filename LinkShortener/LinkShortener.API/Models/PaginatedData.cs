using System.Collections.Generic;

namespace LinkShortener.API.Models
{
    public class PaginatedData<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}