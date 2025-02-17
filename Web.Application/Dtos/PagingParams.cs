using System.ComponentModel.DataAnnotations;

namespace Web.Application.Dtos
{
    public class PagingParams
    {
        [Range(0, int.MaxValue)]
        public int PageIndex { get; set; } = 1;

        [Range(1, 20)]
        public int PageSize { get; set; } = 10;
    }
}
