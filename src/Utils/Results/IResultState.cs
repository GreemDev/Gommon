namespace Gommon;

public interface IResultState;

public struct Success : IResultState
{
    public static readonly Success Shared = new();
}

public interface IErrorState : IResultState;

public readonly struct Error : IErrorState
{
    public static readonly Error Shared = new();
}

public readonly struct MessageError : IErrorState
{
    public MessageError(string message) => Content = message;

    public string Content { get; }

    public override string ToString() => Content;
}

public readonly struct NullError : IErrorState;

public readonly struct GenericError<T> : IErrorState
{
    public T Value { get; }

    public GenericError(T value)
    {
        Value = value;
    }

    public override string ToString() => Value?.ToString() ?? "null";
}