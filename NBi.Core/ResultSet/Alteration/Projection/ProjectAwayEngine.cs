using NBi.Core.ResultSet.Alteration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Projection
{
    class ProjectAwayEngine : ProjectEngine
    {
        public ProjectAwayEngine(ProjectAwayArgs args)
            : base(args) { }

        protected override bool IsColumnToRemove(DataColumn dataColumn, IEnumerable<DataColumn> columns)
            => columns.Contains(dataColumn);
    }
}
