namespace Gommon;

public interface IResultState;

public class Success : IResultState
{
    public static readonly Success Shared = new();
}

public interface IErrorState : IResultState;

public readonly struct MessageError : IErrorState
{
    public MessageError(string message) => Content = message;

    public string Content { get; }

    public override string ToString() => Content;
}

public readonly struct NullError : IErrorState;

public readonly struct ArbitraryError<T> : IErrorState
{
    public T Value { get; }

    public ArbitraryError(T value)
    {
        Value = value;
    }

    public override string ToString() => Value?.ToString() ?? "null";
}