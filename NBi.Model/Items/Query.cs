using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using NBi.Core;
using NBi.Model.Settings;

namespace NBi.Model.Items;

public class Query : Queryable
{
    private SuiteSettings? settings;

    public override SuiteSettings? Settings
    {
        get => settings;
        set
        {
            settings = value;
            if (Assembly != null)
                Assembly.Settings = value;
            if (Report != null)
                Report.Settings = value;
            if (SharedDataset != null)
                SharedDataset.Settings = value;
        }
    }

    public string File { get; set; } = string.Empty;

    public string ConnectionStringReference { get; set; } = string.Empty;

    private string inlineQuery = string.Empty;

    public virtual string InlineQuery
    {
        get { return inlineQuery; }
        set { inlineQuery = value; }
    }

    public AssemblyQuery? Assembly { get; set; }

    public virtual Report? Report { get; set; }

    public virtual SharedDataset? SharedDataset { get; set; }
}
