using Expressif;
using Expressif.Values;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable;

public class Context : IContext
{
    public IEnumerable<IColumnAlias> Aliases { get; } = [];
    public IEnumerable<IColumnExpression> Expressions { get; } = [];

    private static readonly NoneContext noneContext = new ();
    public static Context None => noneContext;

    public virtual ContextVariables Variables { get; }

    private readonly ContextObject currentObject = new();
    public ContextObject CurrentObject => currentObject;
    public virtual IResultRow? CurrentRow => (IResultRow?)currentObject?.Value;

    internal Context()
        : this(new ContextVariables())
    { }

    internal Context(IDictionary<string, IVariable> variables)
    {
        Variables = new();
        foreach (var variable in variables)
            Variables.Add<object>(variable.Key, variable.Value);
    }

    public Context(ContextVariables variables)
        => Variables = variables;

    public Context(ContextVariables variables, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
        : this(variables) => (Aliases, Expressions) = (aliases, expressions);

    public void Switch(IResultRow? currentRow)
        => currentObject.Set(currentRow);

    private class NoneContext : Context
    {
        public override IResultRow? CurrentRow => throw new NotSupportedException();
        public override ContextVariables Variables => throw new NotSupportedException();
    }
}
