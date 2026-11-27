using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;

namespace NBi.Model.Items;

public class ReportBase
{
    public string Source { get; set; }

    public string Path { get; set; }

    public ReportBase()
    {

    }
}
