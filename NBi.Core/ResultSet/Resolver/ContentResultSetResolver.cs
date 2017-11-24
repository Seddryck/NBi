using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    class ContentResultSetResolver : RowsResultSetResolver
    {
        private readonly ContentResultSetResolverArgs args;

        public ContentResultSetResolver(ContentResultSetResolverArgs args)
            : base(args)
        {
            this.args = args;
        }

        public override ResultSet Execute()
        {
            var rs = base.Execute();
            for (int i = 0; i < args.ColumnNames.Count(); i++)
            {
                if (!string.IsNullOrEmpty(args.ColumnNames.ElementAt(i)))
                    rs.Table.Columns[i].ColumnName = args.ColumnNames.ElementAt(i);
            }
            return rs;
        }
    }
}
