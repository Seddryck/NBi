using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Extension
{
    class NCalcExtendEngine : AbstractExtendEngine
    {
        public NCalcExtendEngine(ServiceLocator serviceLocator, IColumnIdentifier newColumn, string code)
            : base(serviceLocator, newColumn, code) { }

        protected override ResultSet Execute(ResultSet rs, int ordinal)
        {
            foreach (DataRow row in rs.Rows)
            {
                var args = new NCalcScalarResolverArgs(Code, row);
                var resolver = new NCalcScalarResolver<object>(args);
                row[ordinal] = resolver.Execute();
            }
            return rs;
        }
    }
}
