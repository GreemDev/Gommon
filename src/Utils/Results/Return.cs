#nullable enable
using System;

namespace Gommon;

/// <summary>
///     Represents the result of a method that can produce errors and returns a value.
/// </summary>
public class Return<T> : Result
{
    public Optional<T> Value { get; }
    
    private Return(IResultState state, T? value) : base(state)
    {
        Value = Optional.Of(value);
    }

    public static implicit operator Return<T>(T value) => new(Gommon.Success.Shared, value);

    public new static Return<T> Failure(IErrorState errorState) => new(errorState, default);

    public new static Return<T> Unspecified(IResultState state) => new(state, default);

    public new T Unwrap()
    {
        base.Unwrap();
        return Value.OrThrow<NullReferenceException>();
    }
    
    public Optional<T> UnwrapOptional(out Exception unwrapped)
    {
        if (TryUnwrapError(out unwrapped!))
            return default;
        
        return Value;
    }
}