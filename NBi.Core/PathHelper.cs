using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core
{
    public class PathHelper
    {
        public string Combine(string basePath, string path, string filename)
        {
            if (Path.IsPathRooted(path) || string.IsNullOrEmpty(basePath))
                return $"{path}{filename}";
            else
                return $"{basePath}{path}{filename}";
        }
    }
}
