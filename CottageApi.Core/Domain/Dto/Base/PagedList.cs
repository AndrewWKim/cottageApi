using System.Collections.Generic;

namespace CottageApi.Core.Domain.Dto.Base
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }

        public int Total { get; set; }
    }
}