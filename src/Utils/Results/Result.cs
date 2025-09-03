using System;
using System.Diagnostics.CodeAnalysis;

namespace Gommon;

/// <summary>
///     Represents the result of a method that can produce errors but does not return a value.
/// </summary>
public class Result
{
    private readonly IResultState _state;

    protected Result(IResultState state)
    {
        _state = state;
    }
    

    public static Result Success { get; } = new(Gommon.Success.Shared);
    
    public static Result Failure(IErrorState state) => new(state);
    
    public static Result Unspecified(IResultState state) => new(state);

    public bool IsError => _state is IErrorState;

    public bool IsSuccess => _state is Success;

    public void Unwrap()
    {
        if (TryUnwrapError(out var exception))
            throw exception;
    }
    
    public bool TryUnwrapError([MaybeNullWhen(false)] out Exception unwrapped)
    {
        if (IsError)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (_state is Exception e)
            {
                unwrapped = e;
            }
            else
            {
                unwrapped = new Exception(_state.ToString());
            }

            return true;
        }

        unwrapped = null;
        return false;
    }
}