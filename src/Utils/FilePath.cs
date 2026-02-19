using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotNetPath = System.IO.Path;

namespace Gommon;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
public readonly record struct FilePath
{
    #region Global common paths

    public static FilePath Logs => new("logs", true);
    public static FilePath Data => new("data", true);

    #endregion

    public string Path { get; }

    public string FullPath => DotNetPath.GetFullPath(Path);

    private readonly Lazy<bool> _isDirLazy;

    public bool IsDirectory => _isDirLazy.Value;

    public FilePath(string path, bool? isDirectory = null)
    {
        Path = path ??
               throw new NullReferenceException($"The path of a constructed {nameof(FilePath)} should not be null.");

        _isDirLazy = new Lazy<bool>(isDirectory is null
            ? () => Directory.Exists(path) && !File.Exists(path)
            : () => isDirectory!.Value
        );
    }

    public FilePath Resolve(string subPath, bool? isDirectory = null)
        => new(
            DotNetPath.Combine(Path, subPath),
            isDirectory
        );


    public static FilePath operator /(FilePath left, string right) => left.Resolve(right);
    public static FilePath operator /(FilePath left, FilePath right) => left.Resolve(right.Path, right.IsDirectory);

    public static FilePath operator --(FilePath curr) =>
        curr.TryGetParent(out var parentDir)
            ? parentDir
            : curr;


    public bool TryGetParent(out FilePath filePath)
    {
        var parentDir = Directory.GetParent(Path);
        if (parentDir == null)
        {
            filePath = default;
            return false;
        }

        filePath = new FilePath(parentDir.FullName, isDirectory: true);
        return true;
    }

    public string Extension
    {
        get
        {
            if (IsDirectory) return null;

            var ext = DotNetPath.GetExtension(Path);
            if (ext!.StartsWith('.'))
                ext = ext.TrimStart('.');

            return ext;
        }
    }

    public string Name =>
        IsDirectory
            ? DirectoryName
            : DotNetPath.GetFileName(Path);

    public string DirectoryName => DotNetPath.GetDirectoryName(Path);

    public string NameWithoutExtension =>
        IsDirectory
            ? DirectoryName
            : DotNetPath.GetFileNameWithoutExtension(Path);

    public bool ExistsAsFile => File.Exists(Path);
    public bool ExistsAsDirectory => Directory.Exists(Path);

    public void CreateAsDirectory()
    {
        Directory.CreateDirectory(Path);
    }

    // copied from stdlib
    internal const int DefaultBufferSize = 4096;

    public FileStream CreateAsFileAndOpen(int bufferSize = DefaultBufferSize, FileOptions? fopt = null)
    {
        if (fopt is not { } fileOptions)
            return File.Create(Path, bufferSize);

        return File.Create(Path, bufferSize, fileOptions);
    }

    public void CreateAsFile() 
        => File.Create(Path, 0, FileOptions.None).Dispose();

    public void TryCreate()
    {
        if (IsDirectory)
            CreateAsDirectory();
        else if (!ExistsAsFile)
            CreateAsFile();
    }

    public IEnumerable<FilePath> EnumerateFiles(string searchPattern)
        => Directory.EnumerateFiles(Path, searchPattern)
            .Select(x => new FilePath(x, isDirectory: false));

    public IEnumerable<FilePath> EnumerateFiles(string searchPattern, SearchOption searchOption)
        => Directory.EnumerateFiles(Path, searchPattern, searchOption)
            .Select(x => new FilePath(x, isDirectory: false));

    public IEnumerable<FilePath> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions)
        => Directory.EnumerateFiles(Path, searchPattern, enumerationOptions)
            .Select(x => new FilePath(x, isDirectory: false));

    public IEnumerable<FilePath> EnumerateDirectories(string searchPattern)
        => Directory.EnumerateDirectories(Path, searchPattern)
            .Select(x => new FilePath(x, isDirectory: true));

    public IEnumerable<FilePath> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        => Directory.EnumerateDirectories(Path, searchPattern, searchOption)
            .Select(x => new FilePath(x, isDirectory: true));

    public IEnumerable<FilePath> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions)
        => Directory.EnumerateDirectories(Path, searchPattern, enumerationOptions)
            .Select(x => new FilePath(x, isDirectory: true));

    public IEnumerable<FilePath> EnumerateFileSystemEntries(string searchPattern)
        => Directory.EnumerateFileSystemEntries(Path, searchPattern)
            .Select(x => new FilePath(x));

    public IEnumerable<FilePath> EnumerateFileSystemEntries(string searchPattern, SearchOption searchOption)
        => Directory.EnumerateFileSystemEntries(Path, searchPattern, searchOption)
            .Select(x => new FilePath(x));

    public IEnumerable<FilePath> EnumerateFileSystemEntries(string searchPattern, EnumerationOptions enumerationOptions)
        => Directory.EnumerateFileSystemEntries(Path, searchPattern, enumerationOptions)
            .Select(x => new FilePath(x));

    public List<FilePath> GetFiles() =>
        IsDirectory
            ? Directory.GetFiles(Path).Select(path => new FilePath(path, false)).ToList()
            : null;

    public List<FilePath> GetSubdirectories() =>
        IsDirectory
            ? Directory.GetDirectories(Path).Select(path => new FilePath(path, true)).ToList()
            : null;

    public void WriteAllText(string text) => File.WriteAllText(Path, text);
    public void WriteAllBytes(byte[] bytes) => File.WriteAllBytes(Path, bytes);
    public void WriteAllLines(IEnumerable<string> lines) => File.WriteAllLines(Path, lines);

    public void AppendAllText(string text) => File.AppendAllText(Path, text);
    public void AppendAllLines(IEnumerable<string> lines) => File.AppendAllLines(Path, lines);

    public string ReadAllText(Encoding encoding = null) => File.ReadAllText(Path, encoding ?? Encoding.UTF8);
    public string[] ReadAllLines(Encoding encoding = null) => File.ReadAllLines(Path, encoding ?? Encoding.UTF8);
    public byte[] ReadAllBytes() => File.ReadAllBytes(Path);

    public FileStream OpenRead() => File.OpenRead(Path);
    public FileStream OpenWrite() => File.OpenWrite(Path);

    public override string ToString() => Path;

    public static implicit operator FilePath(string path) => new(path);
    public static implicit operator string(FilePath fp) => fp.ToString();
    public static implicit operator FilePath(DirectoryInfo di) => new(di.FullName, true);
    public static implicit operator FilePath(FileInfo fi) => new(fi.FullName, false);

    public static implicit operator DirectoryInfo(FilePath fp) => new(fp.FullPath);
    public static implicit operator FileInfo(FilePath fp) => new(fp.FullPath);
}

public static class FilePathExtensions
{
    public static FilePath Resolve(this string root, string subPath, bool? isDirectory = null) 
        => new FilePath(root).Resolve(subPath, isDirectory);
}