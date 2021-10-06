using NBi.Core.ResultSet.Alteration;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    class ResultSetService : IResultSetService
    {
        protected readonly Load load;
        protected readonly IReadOnlyList<Alter> alterations;
        public IReadOnlyList<Alter> Alterations { get => alterations; }
        public Load Load { get => load; }

        public ResultSetService(Load load, List<Alter> alterations)
        {
            this.load = load;
            this.alterations = (alterations ?? new List<Alter>()).AsReadOnly();
        }

        public IResultSet Execute()
        {
            var rs = load.Invoke();
            foreach (var alteration in alterations)
                rs = alteration.Invoke(rs);
            return rs;
        }
    }
}
