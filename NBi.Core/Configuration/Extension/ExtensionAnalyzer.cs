using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Configuration.Extension
{
    public class ExtensionAnalyzer
    {
        public Type[] Execute(string name)
        {
            var types = Assembly.Load(name).GetTypes();

            var interfaces = new[]
            {
                typeof(Query.Client.IClientFactory),
                typeof(Query.Command.ICommandFactory),
                typeof(Query.Execution.IExecutionEngine),
                typeof(Query.Performance.IPerformanceEngine),
                typeof(Query.Validation.IValidationEngine),
                typeof(Query.Format.IFormatEngine),
            };

            var notables = new List<Type>();
            foreach (var @interface in interfaces)
                notables.AddRange(types.Where(x => @interface.IsAssignableFrom(x)));

            return notables.ToArray();
        }
    }
}
