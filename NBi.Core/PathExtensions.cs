using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core;

static class PathExtensions
{
    public static string CombineOrRoot(string basePath, string path, string filename)
    {
        if (Path.IsPathRooted(path) || string.IsNullOrEmpty(basePath))
            return Path.Combine(path, filename);
        else
            return Path.Combine(basePath, path, filename);
    }

    public static string CombineOrRoot(string basePath, string path)
    {
        if (Path.IsPathRooted(path) || string.IsNullOrEmpty(basePath))
            return AddBackslash(path);
        else
            return AddBackslash(Path.Combine(basePath, path));
    }

    public static string AddBackslash(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException();

        var lastChar = path.ToCharArray().Last();
        if (lastChar == Path.DirectorySeparatorChar)
            return path;
        else
            return $@"{path}\";
    }
}
