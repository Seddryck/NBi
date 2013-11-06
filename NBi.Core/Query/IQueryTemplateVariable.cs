using System;
using System.Linq;

namespace NBi.Core.Query
{
    public interface IQueryTemplateVariable
    {
        string Name { get; set; }
        string Value { get; set; }
    }
}
