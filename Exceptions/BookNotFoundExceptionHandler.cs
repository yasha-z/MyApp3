using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Session1
{
    // TASK 3.7 — Converts BookNotFoundException into HTTP 404 with RFC 7807 ProblemDetails body.
    public class BookNotFoundExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not BookNotFoundException notFound)
            {
                return false;
            }

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Book Not Found",
                Detail = notFound.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            };

            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }
    }
}
