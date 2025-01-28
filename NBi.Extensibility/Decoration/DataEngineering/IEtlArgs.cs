using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Extensibility.Decoration.DataEngineering;

public interface IEtlArgs
{
    string Version { get; set; }
    string Server { get; set; }
    string Path { get; set; }
    string Name { get; set; }
    string UserName { get; set; }
    string Password { get; set; }
    IDictionary<string, object> Parameters { get; }
    string Catalog { get; set; }
    string Folder { get; set; }
    string Project { get; set; }
    string Environment { get; set; }
    bool Is32Bits { get; set; }
    int Timeout { get; set; }
}
