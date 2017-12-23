using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Configuration
{
    public class ExtensionsAnalyzer
    {
        public Type[] Analyze(string name)
        {
            var types = Assembly.Load(name).GetTypes();

            var interfaces = new[]
            {
                typeof(Query.Session.ISessionFactory),
                typeof(Query.Command.ICommandFactory),
                typeof(Query.Execution.IExecutionEngine),
                typeof(Query.Performance.IPerformanceEngine),
                typeof(Query.Validation.IValidationEngine),
                typeof(Query.Format.IFormatEngine),
            };

            var notables = new List<Type>();
            foreach (var @interface in interfaces)
                notables.AddRange(types.Where(x => x.IsAssignableFrom(@interface)));

            return notables.ToArray();
        }
    }
}
