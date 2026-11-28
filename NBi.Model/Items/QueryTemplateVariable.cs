using System;
using System.Linq;
using NBi.Extensibility.Query;

namespace NBi.Model.Items;

public class QueryTemplateVariable : IQueryTemplateVariable
{
    public string Name { get; set; }

    public string Value { get; set; }
}
