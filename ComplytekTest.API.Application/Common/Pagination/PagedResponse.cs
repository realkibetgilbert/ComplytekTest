using ComplytekTest.API.Application.Common.Responses;

namespace ComplytekTest.API.Application.Common.Pagination
{
    public class PagedResponse<T> : ApiResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
        public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords, int totalPages, Uri firstPage, Uri lastPage, Uri nextPage, Uri previousPage)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = totalPages;
            FirstPage = firstPage;
            LastPage = lastPage;
            NextPage = nextPage;
            PreviousPage = previousPage;
            IsSuccess = true;
            Title = "Success";
            Message = "Request completed successfully.";
        }
    }
}
