using ComplytekTest.API.Application.Common.Responses;
using ComplytekTest.API.Application.Interfaces;

namespace ComplytekTest.API.Application.Common.Pagination
{
    public static class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedResponse<T>(
            List<T> pagedData,
            PaginationFilter validFilter,
            int totalRecords,
            IUriService uriService,
            string route)
        {
            var totalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize);

            var response = new PagedResponse<List<T>>(
                data: pagedData,
                pageNumber: validFilter.PageNumber,
                pageSize: validFilter.PageSize,
                totalRecords: totalRecords,
                totalPages: totalPages,
                firstPage: uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route),
                lastPage: uriService.GetPageUri(new PaginationFilter(totalPages, validFilter.PageSize), route),
                nextPage: validFilter.PageNumber < totalPages
                    ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                    : null,
                previousPage: validFilter.PageNumber > 1
                    ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                    : null
            )
            {
                IsSuccess = true,
                ErrorCode = ApiErrorCode.None,
                Title = "Success",
                Message = "Request completed successfully."
            };

            return response;
        }
    }
}
