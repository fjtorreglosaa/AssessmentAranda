using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Logic.Extensions
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParametersOnHeader<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double numberOfRecords = await queryable.CountAsync();
            httpContext.Response.Headers.Add("numberOfRecords", numberOfRecords.ToString());
        }
    }
}
