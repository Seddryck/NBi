using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    class ResultSetService : IResultSetService
    {
        protected readonly Func<ResultSet> load;
        protected readonly IEnumerable<Action<ResultSet>> transformations;
        public int TransformationCount { get => transformations.Count(); }
        
        public ResultSetService(
            Func<ResultSet> load,
            IEnumerable<Action<ResultSet>> transformations)
        {
            this.load = load;
            this.transformations = transformations;
        }

        public ResultSet Execute()
        {
            var rs = load.Invoke();
            foreach (var transformation in transformations)
                transformation.Invoke(rs);
            return rs;
        }
    }
}
