using ComplytekTest.API.Application.Common.Pagination;

namespace ComplytekTest.API.Application.Interfaces
{
    public interface IUriService
    {
        Uri GetPageUri(PaginationFilter filter, string route);
    }
}
