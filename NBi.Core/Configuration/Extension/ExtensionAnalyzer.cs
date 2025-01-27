using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.FlatFile;
using NBi.Extensibility.Query;

namespace NBi.Core.Configuration.Extension;

public class ExtensionAnalyzer
{
    public Type[] Execute(string name)
    {
        var types = Assembly.Load(name).GetTypes();

        var interfaces = new[]
        {
            typeof(IClientFactory),
            typeof(ICommandFactory),
            typeof(IExecutionEngine),
            typeof(Query.Performance.IPerformanceEngine),
            typeof(Query.Validation.IValidationEngine),
            typeof(Query.Format.IFormatEngine),
            typeof(IFlatFileReader)
        };

        var notables = new List<Type>();
        foreach (var @interface in interfaces)
            notables.AddRange(types.Where(x => @interface.IsAssignableFrom(x) && !x.IsAbstract && x.IsPublic));

        return [.. notables];
    }
}
