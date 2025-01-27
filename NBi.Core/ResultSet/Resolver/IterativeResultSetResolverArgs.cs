using NBi.Core.ResultSet.Alteration.Merging;
using NBi.Core.Variable;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

public class IterativeResultSetResolverArgs : ResultSetResolverArgs
{
    public IResultSetResolver ResultSetResolver { get; }
    public ISequenceResolver SequenceResolver { get; }
    public IMergingEngine MergingEngine { get; }
    public string VariableName { get; }
    public IDictionary<string, IVariable> Variables { get; }

    public IterativeResultSetResolverArgs(ISequenceResolver sequenceResolver, string variableName, IDictionary<string, IVariable> variables, IResultSetResolver resultSetResolver)
    {
        SequenceResolver = sequenceResolver;
        ResultSetResolver = resultSetResolver;
        MergingEngine = new UnionByOrdinalEngine(resultSetResolver);
        Variables = variables;
        VariableName = variableName;
    }
}
