using System;
using System.Diagnostics.CodeAnalysis;

namespace Gommon;

public interface IResult
{
    public bool IsError { get; }

    public bool IsSuccess { get; }

    public void Unwrap();

    public bool TryUnwrapError([MaybeNullWhen(false)] out Exception unwrapped);

    public bool IsOf<TResultState>() where TResultState : IResultState;
    public bool IsOf<TResultState>(out TResultState resultState) where TResultState : IResultState;
}