using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using NBi.Core;
using NBi.Model.Settings;

namespace NBi.Model.Items;

public abstract class BaseItem
{
    public virtual Default? Default { get; set; }
    public virtual SuiteSettings? Settings { get; set; }

    public BaseItem()
    {
        Default = new Default();
        Settings = new SuiteSettings();
    }

    public string? ConnectionString { get; set; }

    [Obsolete("Replaced by connection-string")]
    public string? ConnectionStringOld
    {
        get => ConnectionString;
        set { ConnectionString = value; }
    }

    public string? Roles { get; set; }
}
