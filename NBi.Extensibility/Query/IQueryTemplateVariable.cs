using System;
using System.Linq;

namespace NBi.Extensibility.Query;

public interface IQueryTemplateVariable
{
    string Name { get; set; }
    string Value { get; set; }
}
