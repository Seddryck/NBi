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

class NativeExtendEngine : AbstractExtendEngine
{
    public NativeExtendEngine(ServiceLocator serviceLocator, Context context, IColumnIdentifier newColumn, string code)
        : base(serviceLocator, context, newColumn, code) { }

    protected override IResultSet Execute(IResultSet rs, int ordinal)
    {
        var argsFactory = new ScalarResolverArgsFactory(ServiceLocator, Context);
        var args = argsFactory.Instantiate(Code);
        var factory = ServiceLocator.GetScalarResolverFactory();
        var resolver = factory.Instantiate(args);

        foreach (var row in rs.Rows)
        {
            Context.Switch(row);
            row[ordinal] = resolver.Execute();
        }
        return rs;
    }
}
