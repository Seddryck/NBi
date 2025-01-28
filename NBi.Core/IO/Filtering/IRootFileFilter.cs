using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.IO.Filtering;

public interface IRootFileFilter : IFileFilter
{
    FileInfo[] Execute(string path);
}
