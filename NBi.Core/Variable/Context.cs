using Expressif;
using Expressif.Values;
using NBi.Core.Evaluate;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class Context : IContext
    {

        public IEnumerable<IColumnAlias> Aliases { get; } = [];
        public IEnumerable<IColumnExpression> Expressions { get; } = [];

        public virtual IDictionary<string, IVariable> Variables { get; } = new Dictionary<string, IVariable>();
        public virtual IResultRow? CurrentRow => (IResultRow?)currentObject.Value;

        private static readonly NoneContext noneContext = new ();
        public static Context None => noneContext;

        ContextVariables IContext.Variables => throw new NotImplementedException();

        private readonly ContextObject currentObject = new();
        public ContextObject CurrentObject => currentObject;

        internal Context()
            : this(new Dictionary<string, IVariable>())
        { }

        public Context(IDictionary<string, IVariable> variables)
            => Variables = variables;

        public Context(IDictionary<string, IVariable> variables, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
            : this(variables) => (Aliases, Expressions) = (aliases, expressions);

        public void Switch(IResultRow currentRow)
            => currentObject.Set(currentRow);

        private class NoneContext : Context
        {
            public override IResultRow? CurrentRow => throw new NotSupportedException();
            public override IDictionary<string, IVariable> Variables => throw new NotSupportedException();
        }
    }
}
