using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Resolver
{
    public class ReportDataSetQueryResolverArgs : BaseQueryResolverArgs
    {
        private readonly string source;
        private readonly string path;
        private readonly string name;
        private readonly string dataSetName;

        public string Source { get => source; }
        public string Path { get => path; }
        public string Name { get => name; }
        public string DataSetName { get => dataSetName; }

        public ReportDataSetQueryResolverArgs(string source, string path, string name, string dataSetName,
            string connectionString, IEnumerable<IQueryParameter> parameters,
            IEnumerable<IQueryTemplateVariable> variables, TimeSpan timeout)
            : base(connectionString, parameters, variables, timeout)
        {
            this.source = source;
            this.path = path;
            this.name = name;
            this.dataSetName = dataSetName;
        }
    }
}
