using System.Collections.Generic;
using System.Data;
using System.IO;
using NBi.Core;

namespace NBi.Model.Items;

public class AssemblyParameter
{
    public string Name { get; set; }

    public string StringValue { get; set; }

    public object Value
    {
        get
        {
            return StringValue;
        }
    }
}
