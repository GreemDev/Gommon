#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace Gommon;

/// <summary>
///     Represents the result of a method that can produce errors and returns a value.
/// </summary>
public readonly struct Return<T> : IResult
{
    private readonly IResultState _state;

    public Optional<T> Value { get; }

    private Return(IResultState state, T? value)
    {
        _state = state;
        Value = Optional.Of(value);
    }

    public Return()
    {
        _state = Success.Shared;
        Value = Optional<T>.None;
    }

    public static implicit operator Return<T>(T value) => new(Success.Shared, value);

    public static Return<T> Failure(IErrorState errorState) => new(errorState, default);

    public static Return<T> Unspecified(IResultState state) => new(state, default);

    public bool IsError => _state is IErrorState;
    public bool IsSuccess => _state is Success;

    public static implicit operator bool(Return<T> ret) => ret.IsSuccess;

    public bool IsOf<TResultState>() where TResultState : IResultState => _state is TResultState;

    public bool IsOf<TResultState>([MaybeNullWhen(false)] out TResultState resultState) 
        where TResultState : IResultState
    {
        resultState = _state.Cast<TResultState>();

        return _state is TResultState;
    }

    public T Unwrap()
    {
        ((IResult)this).Unwrap();
        return Value.OrThrow("Return<T> has no value.");
    }

    public bool TryUnwrap(out T? value, out Exception? error) 
        => UnwrapOptional(out error).TryGet(out value);

    public Optional<T> UnwrapOptional(out Exception unwrapped)
    {
        if (((IResult)this).TryUnwrapError(out unwrapped!))
            return default;

        return Value;
    }

    void IResult.Unwrap()
    {
        if (((IResult)this).TryUnwrapError(out var exception))
            throw exception;
    }

    bool IResult.TryUnwrapError([MaybeNullWhen(false)] out Exception unwrapped)
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