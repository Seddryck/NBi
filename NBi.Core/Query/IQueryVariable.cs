using System;
using System.Linq;

namespace NBi.Core.Query
{
    public interface IQueryVariable
    {
        string Name { get; set; }
        string Value { get; set; }
    }
}
