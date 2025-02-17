using System.Net;

namespace Web.Application.Dtos
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; } = false;
        public HttpStatusCode StatusCode { get; set; }
        public string? ErrorMessage { get; set; }    
    }

    public class DataResult<T> : ApiResult where T : class
    {
        public T? Data { get; set; }
    }

    public class ListDataResult<T> : ApiResult where T : class
    {
        public List<T>? Data { get; set; }
        public int TotalCount { 
            get {
                return Data?.Count() ?? 0;
            } 
        }
    }
    public class PagingDataResult<T> : ApiResult where T : class
    {
        public IList<T>? Data { get; set; }
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }
    }
}
