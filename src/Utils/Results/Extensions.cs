using System;
using System.Collections.Generic;
using System.Linq;

namespace Gommon;

public static class ResultExtensions
{
    public static (IEnumerable<TResultType> Successful, IEnumerable<TResultType> Failures) 
        SplitSuccessAndFailure<TResultType>(
            this ICollection<TResultType> returns
        ) where TResultType : IResult
        => (
            returns.Where(x => x.IsSuccess),
            returns.Where(x => x.IsError)
        );

    public static IEnumerable<TResultType> OnlySuccessful<TResultType>(this ICollection<TResultType> results)
        where TResultType : IResult
        => results.Where(x => x.IsSuccess);

    public static IEnumerable<TResultType> OnlyFailures<TResultType>(this ICollection<TResultType> results)
        where TResultType : IResult
        => results.Where(x => x.IsError);

    public static IEnumerable<Optional<T>> UnwrapAllToOptionals<T>(this ICollection<Return<T>> results)
        => results.Select(x => x.UnwrapOptional(out _));

    public static IEnumerable<T> UnwrapAll<T>(this ICollection<Return<T>> results)
        => results.Select(x => x.Unwrap());

    /// <remarks>Can contain null elements.</remarks>
    public static IEnumerable<Exception> UnwrapAllToExceptions<T>(this ICollection<Return<T>> results)
        => results.Select(x => !x.UnwrapOptional(out var exc).HasValue ? exc : null);
}