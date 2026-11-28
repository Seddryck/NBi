using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Items;

public class EtlBase
{
    protected const int DEFAULT_TIMEOUT = -1;

    public string Version { get; set; }

    public string Server { get; set; }

    public string Path { get; set; }

    public string Name { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string Catalog { get; set; }

    public string Folder { get; set; }

    public string Project { get; set; }

    public string Environment { get; set; }

    internal bool IsEmpty()
    {
        return string.IsNullOrEmpty(Server) && string.IsNullOrEmpty(Path) && string.IsNullOrEmpty(Name)
            && string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(Catalog)
            && string.IsNullOrEmpty(Folder) && string.IsNullOrEmpty(Project) && string.IsNullOrEmpty(Environment)
            && !Is32BitsSpecified && !TimeoutSpecified;
    }

    public bool Is32Bits { get; set; }

    public bool Is32BitsSpecified { get; set; }

    public int Timeout { get; set; }

    public bool TimeoutSpecified { get; set; }

    public EtlBase()
    {
    }
}
