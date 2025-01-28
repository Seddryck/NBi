using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Report;

public class SharedDatasetRequest
{
    public string Source { get; set; }
    public string Path { get; set; }
    public string SharedDatasetName { get; set; }

    public SharedDatasetRequest(string source, string path, string name)
    {
        Source = source;
        Path = path;
        SharedDatasetName = name;
    }
}
