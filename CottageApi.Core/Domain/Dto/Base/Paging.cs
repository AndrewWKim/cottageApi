namespace CottageApi.Core.Domain.Dto.Base
{
    public class Paging
    {
        public int Page { get; set; }

        public int PageSize { get; set; } = 10;

        public int Skip => Page * PageSize;
    }
}