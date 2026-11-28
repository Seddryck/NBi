using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.FlatFile;
using NBi.Extensibility.Query;

namespace NBi.CSharp.Assemblies;

internal static class ExtensionDiscovery
{
    internal static Type[] Execute(string name)
    {
        var types = Assembly.Load(name).GetTypes();

        var interfaces = new[]
        {
            typeof(IClientFactory),
            typeof(ICommandFactory),
            typeof(IExecutionEngine),
            typeof(NBi.Core.Query.Performance.IPerformanceEngine),
            typeof(NBi.Core.Query.Validation.IValidationEngine),
            typeof(NBi.Core.Query.Format.IFormatEngine),
            typeof(IFlatFileReader)
        };

        var notables = new List<Type>();
        foreach (var @interface in interfaces)
            notables.AddRange(types.Where(x => @interface.IsAssignableFrom(x) && !x.IsAbstract && x.IsPublic));

        return [.. notables];
    }
}
