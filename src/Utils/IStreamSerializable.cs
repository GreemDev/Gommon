using System;
using System.IO;

namespace Gommon;

public interface IStreamSerializable<out T> where T : IStreamSerializable<T>, new()
{
    public void Read(Stream stream);
    public void Write(Stream stream);
    
    public static virtual T ReadFrom(Stream stream)
    {
        if (!stream.CanRead)
            throw new ArgumentException("Stream must be readable.", nameof(stream));

        if (!stream.CanSeek)
            throw new ArgumentException("Stream must be seekable.", nameof(stream));

        stream.Seek(0, SeekOrigin.Begin);

        var result = new T();

        result.Read(stream);

        return result;
    }

    public static virtual T ReadFromPassive(Stream stream)
    {
        if (!stream.CanRead)
            throw new ArgumentException("Stream must be readable.", nameof(stream));

        var result = new T();

        result.Read(stream);

        return result;
    }
}

public static class StreamSerializableExtensions
{
    public static T ReadEntirelyAs<T>(this Stream stream) where T : IStreamSerializable<T>, new() 
        => T.ReadFrom(stream);

    public static T ReadAs<T>(this Stream stream) where T : IStreamSerializable<T>, new() 
        => T.ReadFromPassive(stream);

    public static void Write<T>(this Stream stream, T data) where T : IStreamSerializable<T>, new()
        => data.Write(stream);
}