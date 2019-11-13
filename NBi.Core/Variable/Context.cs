using NBi.Core.Evaluate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class Context
    {

        public IEnumerable<IColumnAlias> Aliases { get; } = new List<IColumnAlias>();
        public IEnumerable<IColumnExpression> Expressions { get; } = new List<IColumnExpression>();

        public IDictionary<string, ITestVariable> Variables { get; } = new Dictionary<string, ITestVariable>();
        public DataRow CurrentRow { get; private set; }

        public static Context None { get; } = new Context();

        private Context() { }

        public Context(IDictionary<string, ITestVariable> variables)
            => Variables = variables;

        public Context(IDictionary<string, ITestVariable> variables, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            : this(variables) => (Aliases, Expressions) = (aliases, expressions);

        public void Switch(DataRow currentRow)
            => CurrentRow = currentRow;
    }
}
