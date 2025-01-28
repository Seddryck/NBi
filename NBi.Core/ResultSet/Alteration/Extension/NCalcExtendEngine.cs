using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Extension;

class NCalcExtendEngine : AbstractExtendEngine
{
    public NCalcExtendEngine(ServiceLocator serviceLocator, Context context, IColumnIdentifier newColumn, string code)
        : base(serviceLocator, context, newColumn, code) { }

    protected override IResultSet Execute(IResultSet rs, int ordinal)
    {
        foreach (var row in rs.Rows)
        {
            Context.Switch(row);
            var args = new NCalcScalarResolverArgs(Code, Context);
            var resolver = new NCalcScalarResolver<object>(args);
            row[ordinal] = resolver.Execute();
        }
        return rs;
    }
}
