using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotNetPath = System.IO.Path;

namespace Gommon;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
public record FilePath
{
    #region Global common paths

    public static FilePath Logs => new("logs", true);
    public static FilePath Data => new("data", true);

    #endregion

    public string Path { get; }

    public string FullPath => DotNetPath.GetFullPath(Path);
    
    public bool IsDirectory { get; }

    public FilePath(string path, bool? isDirectory = null)
    {
        Path = path;
        IsDirectory = isDirectory ?? (Directory.Exists(path) && !File.Exists(path));
    }
    
    public FilePath Resolve(string subPath, bool? isDirectory = null)
        => new(
            DotNetPath.Combine(Path, subPath),
            isDirectory
        );

    
    public static FilePath operator /(FilePath left, string right) => left.Resolve(right);

    public static FilePath operator --(FilePath curr) =>
        curr.TryGetParent(out var parentDir)
            ? parentDir
            : curr;


    public bool TryGetParent(out FilePath filePath)
    {
        var parentDir = Directory.GetParent(Path);
        if (parentDir == null)
        {
            filePath = null;
            return false;
        }

        filePath = new FilePath(parentDir.FullName);
        return true;
    }

    public string Extension
    {
        get
        {
            if (IsDirectory) return null;

            var ext = DotNetPath.GetExtension(Path);
            if (ext.StartsWith('.'))
                ext = ext.TrimStart('.');

            return ext;
        }
    }
    
    public string Name =>
        IsDirectory 
            ? DotNetPath.GetDirectoryName(Path) 
            : DotNetPath.GetFileName(Path);

    public string NameWithoutExtension =>
        IsDirectory 
            ? DotNetPath.GetDirectoryName(Path) 
            : DotNetPath.GetFileNameWithoutExtension(Path);

    public bool ExistsAsFile => File.Exists(Path);
    public bool ExistsAsDirectory => Directory.Exists(Path);

    public void Create()
    {
        if (IsDirectory)
            Directory.CreateDirectory(Path);
        else if (!ExistsAsFile)
            File.Create(Path).Dispose();
    }

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

    public FileStream OpenCreate()
        => !ExistsAsFile ? File.Create(Path) : null;

    public override string ToString() => Path;

    public static implicit operator string(FilePath fp) => fp.ToString();
    public static implicit operator FilePath(DirectoryInfo di) => new(di.FullName, true);
    public static implicit operator FilePath(FileInfo fi) => new(fi.FullName, false);
}
