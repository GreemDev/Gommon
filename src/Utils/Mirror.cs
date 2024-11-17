using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable MemberCanBePrivate.Global
namespace Gommon;

public static class Mirror {
    /// <summary>
    ///     Creates a new <see cref="Mirror{T}"/> that will silently fail if no matching type member is found.
    /// </summary>
    /// <param name="obj">The instance of <typeparamref name="T"/> to reflect on.</param>
    /// <returns>A <see cref="Mirror{T}"/> providing easy reflection to access private members.</returns>
    public static Mirror<T> Reflect<T>(T obj) => new(obj);

    /// <summary>
    ///     Creates a new <see cref="Mirror{T}"/> that will fail if no matching type member is found.
    /// </summary>
    /// <param name="obj">The instance of <typeparamref name="T"/> to reflect on.</param>
    /// <returns>A <see cref="Mirror{T}"/> providing easy reflection to access private members, that will error when an attempt is made to access/call a nonexistent member.</returns>
    public static Mirror<T> ReflectUnsafe<T>(T obj) => new(obj, true);
}

/// <summary>
///     A struct holding a value to perform Reflection operations on it with ease.
///     Cannot be used for static.
/// </summary>
public readonly struct Mirror<T>
{
    public T BackingObject { get; }
    private readonly bool _throwIfNullMember;

    internal Mirror(T backingObject, bool throwWhenUnknownMember = false) {
        BackingObject = Optional.Of(backingObject)
            .OrThrow(() => new ValueException($"Cannot create a \"spineless\" ${typeof(Mirror).AsPrettyString()}."));
        _throwIfNullMember = throwWhenUnknownMember;
    }

    private static BindingFlags Fix(BindingFlags? @in)
    {
        if (@in is null)
            return BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
        
        if (@in.Value.HasFlag(BindingFlags.Static))
            throw new InvalidOperationException($"Cannot use a {typeof(Mirror<T>).AsPrettyString()} for static members.");

        return @in.Value.HasFlag(BindingFlags.Instance)
            ? @in.Value
            : @in.Value | BindingFlags.Instance;
    }

    /// <summary>
    ///     Call a method with the given name and <see cref="BindingFlags"/>, with the provided args; ignoring its return value.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="flags">The <see cref="BindingFlags"/> to use.</param>
    /// <param name="args">The arguments to pass to the method.</param>
    public void Call(string name, BindingFlags? flags = null, params object[] args)
        => CallSafe<Missing>(name, flags, args);

    /// <summary>
    ///     Call a method with the given name and <see cref="BindingFlags"/>, with the provided args;
    ///     returning its value as a value of <typeparamref name="TResult"/>, and throwing an exception if it is null under any circumstance.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="flags">The <see cref="BindingFlags"/> to use.</param>
    /// <param name="args">The arguments to pass to the method.</param>
    public TResult Call<TResult>(string name, BindingFlags? flags = null, params object[] args)
        => CallSafe<TResult>(name, flags, args).OrThrow();

    /// <summary>
    ///     Call a method safely with the given name and <see cref="BindingFlags"/>, with the provided args;
    ///     returning its value as a value of <see cref="Optional{TResult}"/>.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="flags">The <see cref="BindingFlags"/> to use.</param>
    /// <param name="args">The arguments to pass to the method.</param>
    public Optional<TResult> CallSafe<TResult>(string name, BindingFlags? flags = null, params object[] args) {
        if (typeof(T).TryGetMethod(name, Fix(flags), out var method))
            return method.Invoke(BackingObject, args).Cast<TResult>();

        if (_throwIfNullMember)
            throw new InvalidOperationException(
                $"Method \"{name}\" does not exist on type {typeof(T).AsPrettyString()} with the given BindingFlags.");
            
        return Optional<TResult>.None;
    }
        
    /// <summary>
    ///     Call a generic method with the given name and <see cref="BindingFlags"/>, with the provided args; ignoring its return value.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="genericTypes">The types, in the correct order, of the generic method.</param>
    /// <param name="flags">The <see cref="BindingFlags"/> to use.</param>
    /// <param name="args">The arguments to pass to the method.</param>
    public void CallGeneric(string name, IEnumerable<Type> genericTypes, BindingFlags? flags = null, params object[] args)
        => CallGenericSafe<object>(name, genericTypes, flags, args);

    /// <summary>
    ///     Call a generic method with the given name and <see cref="BindingFlags"/>, with the provided args;
    ///     returning its value as a value of <typeparamref name="TResult"/>, and throwing an exception if it is null under any circumstance.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="genericTypes">The types, in the correct order, of the generic method.</param>
    /// <param name="flags">The <see cref="BindingFlags"/> to use.</param>
    /// <param name="args">The arguments to pass to the method.</param>
    public TResult CallGeneric<TResult>(string name, IEnumerable<Type> genericTypes, BindingFlags? flags = null, params object[] args)
        => CallGenericSafe<TResult>(name, genericTypes, flags, args).OrThrow();
        
    /// <summary>
    ///     Call a generic method safely with the given name and <see cref="BindingFlags"/>, with the provided args;
    ///     returning its value as a value of <see cref="Optional{TResult}"/>.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="genericTypes">The types, in the correct order, of the generic method.</param>
    /// <param name="flags">The <see cref="BindingFlags"/> to use.</param>
    /// <param name="args">The arguments to pass to the method.</param>
    public Optional<TResult> CallGenericSafe<TResult>(string name, IEnumerable<Type> genericTypes, BindingFlags? flags = null, params object[] args) {
        if (typeof(T).TryGetMethod(name, Fix(flags), out var method))
        {
            if (method.IsGenericMethodDefinition)
            {
                return method.MakeGenericMethod(genericTypes.ToArray())
                    .Invoke(BackingObject, args).Cast<TResult>();
            }
        }

        if (_throwIfNullMember)
            throw new InvalidOperationException(
                $"Method \"{name}\"<{
                    genericTypes.Select(x => x.AsFullNamePrettyString())
                        .JoinToString(", ")
                }> does not exist on type {typeof(T).AsPrettyString()} with the given BindingFlags.");
            
        return Optional<TResult>.None;
    }

    /// <summary>
    ///     Get the value of the given field or property, and throw an exception if it is null under any circumstance.
    /// </summary>
    /// <param name="name">The name of the field or property.</param>
    /// <param name="flags">The <see cref="BindingFlags"/> to use.</param>
    public TResult Get<TResult>(string name, BindingFlags? flags = null) =>
        GetSafe<TResult>(name, flags).OrThrow();

    /// <summary>
    ///     Get the value of the given field or property name and <see cref="BindingFlags"/>.
    /// </summary>
    /// <param name="name">The name of the field or property.</param>
    /// <param name="flags">The <see cref="BindingFlags"/> to use.</param>
    public Optional<TResult> GetSafe<TResult>(string name, BindingFlags? flags = null) {
        if (typeof(T).TryGetField(name, Fix(flags), out var field))
            return field.GetValue(BackingObject).Cast<TResult>();
            
        if (typeof(T).TryGetProperty(name, Fix(flags), out var property))
            return property.GetValue(BackingObject).Cast<TResult>();
            
        if (_throwIfNullMember)
            throw new InvalidOperationException(
                $"Neither a field or property \"{name}\" exists on type {typeof(T).AsPrettyString()} with the given BindingFlags.");
            
        return Optional<TResult>.None;
    }
}