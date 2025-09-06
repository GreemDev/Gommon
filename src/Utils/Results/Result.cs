using System;
using System.Diagnostics.CodeAnalysis;

namespace Gommon;

/// <summary>
///     Represents the result of a method that can produce errors but does not return a value.
/// </summary>
public readonly struct Result : IResult
{
    private readonly IResultState _state;

    private Result(IResultState state)
    {
        _state = state;
    }

    public Result() : this(Gommon.Success.Shared)
    {
    }

    public static Result Success { get; } = new();

    /// <remarks>If <paramref name="state"/> is null, the shared <see cref="Error"/> is used.</remarks>
    public static Result Failure(IErrorState state = null) => new(state ?? Error.Shared);

    public static Result Unspecified(IResultState state) => new(state);

    public bool IsError => _state is IErrorState;

    public bool IsSuccess => _state is Success;

    public bool IsOf<TResultState>() where TResultState : IResultState => _state is TResultState;

    public bool IsOf<TResultState>([MaybeNullWhen(false)] out TResultState resultState)
        where TResultState : IResultState
    {
        resultState = _state.Cast<TResultState>();

        return _state is TResultState;
    }

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