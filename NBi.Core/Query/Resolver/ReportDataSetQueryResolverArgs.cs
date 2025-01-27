using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Resolver;

public class ReportDataSetQueryResolverArgs : BaseQueryResolverArgs
{
    public string Source { get; }
    public string Path { get; }
    public string Name { get; }
    public string DataSetName { get; }

    public ReportDataSetQueryResolverArgs(string source, string path, string name, string dataSetName,
        string connectionString, IEnumerable<IQueryParameter> parameters,
        IEnumerable<IQueryTemplateVariable> variables, TimeSpan timeout)
        : base(connectionString, parameters, variables, timeout)
    {
        Source = source;
        Path = path;
        Name = name;
        DataSetName = dataSetName;
    }
}
