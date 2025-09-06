using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// ReSharper disable MemberCanBePrivate.Global
namespace Gommon;

#nullable enable

/// <summary>
///     Provides common value check methods, such as nullability, collection length, and conditions.
/// </summary>
public static class Guard
{
    public static void Incomplete(Task? task)
    {
        Ensure(!Require(task, "Task").IsCompleted, "Task is already completed, successfully or unsuccessfully.");
    }

    public static void Completed(Task? task)
    {
        Ensure(Require(task, "Task").IsCompleted, "Task is not completed, successfully or unsuccessfully.");
    }

    public static void ValidSnowflake(string? snowflake) => ValidSnowflake(snowflake, $"{snowflake}");

    public static void ValidSnowflake(string? snowflake, string value)
    {
        var nw = Require(snowflake, value).Replace(" ", "");
        if (!nw.All(char.IsNumber))
            ValueException.Throw($"{value} is not a valid snowflake string! Provided: \"{snowflake}\"");
    }

    public static void Ensure(bool expression)
    {
        if (!expression)
            ValueException.Throw("Expression does not equal true.");
    }

    public static void Ensure(bool expression, string message)
    {
        if (!expression)
            ValueException.Throw(message);
    }

    public static void Ensure(bool expression, string message, params object?[] args)
        => Ensure(expression, message.Format(args));

    public static void Matches(string input, string regex, string? name = null)
    {
        RequireObject(input, name);
        RequireObject(regex, name ?? "Regular expression");
        Ensure(new Regex(regex).IsMatch(input),
            $"{name ?? "Input"} must match regex ^{regex}$. Provided: \"{input}\"");
    }

    public static void NotEmpty<T>(IEnumerable<T> coll, string message, params object[] args)
        => NotEmpty(coll, message.Format(args));

    public static void NotEmpty<T>(IEnumerable<T> coll, string message)
    {
        if (coll.None())
            ValueException.Throw(message);
    }

    public static void NoneNull<T>(IEnumerable<T> coll, string message, params object[] args)
        => NoneNull(coll, message.Format(args));

    public static void NoneNull<T>(IEnumerable<T> coll, string message)
    {
        if (coll.Any(x => x is null))
            ValueException.Throw(message);
    }

    public static void RequireObject(object? value) => RequireObject(value, null);

    public static void RequireObject(object? value, string? name)
        => Optional.Of(value).OrThrow(() => new ValueException($"{name ?? "Value"} must not be null."));

    [return: NotNull]
    public static T Require<T>(T value) => Require(value, null);

    [return: NotNull]
    public static T Require<T>(T value, string? name)
        => Optional.Of(value).OrThrow(() => new ValueException($"{name ?? "Value"} must not be null."));
}

public class ValueException : Exception
{
    public static void Throw(string? message = null, Exception? innerException = null)
        => throw new ValueException(message, innerException);

    public ValueException(string? message = null, Exception? innerException = null) :
        base(message, innerException)
    {
    }
}