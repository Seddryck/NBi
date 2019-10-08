using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Extension
{
    class NativeExtendEngine : AbstractExtendEngine
    {
        public NativeExtendEngine(ServiceLocator serviceLocator, IColumnIdentifier newColumn, string code)
            : base(serviceLocator, newColumn, code) { }

        protected override ResultSet Execute(ResultSet rs, int ordinal)
        {
            var context = new Context(null);
            var argsFactory = new ScalarResolverArgsFactory(ServiceLocator, context);
            var args = argsFactory.Instantiate(Code);
            var factory = ServiceLocator.GetScalarResolverFactory();
            var resolver = factory.Instantiate(args);

            foreach (DataRow row in rs.Rows)
            {
                context.Switch(row);
                row[ordinal] = resolver.Execute();
            }
            return rs;
        }
    }
}
