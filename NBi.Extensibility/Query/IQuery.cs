using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Xml.Serialization;

namespace NBi.Extensibility.Query;

public interface IQuery
{
    string Statement { get;}
    TimeSpan Timeout { get; }
    string ConnectionString { get; }
    IEnumerable<IQueryParameter> Parameters { get; }
    IEnumerable<IQueryTemplateVariable> TemplateTokens { get; }
    CommandType CommandType { get; }
}
