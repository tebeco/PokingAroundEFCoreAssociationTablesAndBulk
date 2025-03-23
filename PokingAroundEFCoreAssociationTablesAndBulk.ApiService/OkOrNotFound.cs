using Microsoft.AspNetCore.Http.HttpResults;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Http;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class OkOrNotFoundExtensions
{
    public static Results<NotFound, Ok<TOut>> OkOrNotFound<TIn, TOut>(this IResultExtensions resultExtensions, TIn? value, Func<TIn, TOut> mapper)
        where TIn : notnull
    {
        return value is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(mapper(value));
    }
}
