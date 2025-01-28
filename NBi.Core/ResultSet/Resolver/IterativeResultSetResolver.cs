using NBi.Core.ResultSet.Alteration.Merging;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

class IterativeResultSetResolver : IResultSetResolver
{
    private IResultSetResolver ResultSetResolver { get; }
    private ISequenceResolver SequenceResolver { get; }
    private IMergingEngine MergingEngine { get; }
    private string VariableName { get; }
    private IDictionary<string, IVariable> Variables { get; }

    public IterativeResultSetResolver(ISequenceResolver sequenceResolver, string variableName, IDictionary<string, IVariable> variables, IResultSetResolver resultSetResolver)
    {
        SequenceResolver = sequenceResolver;
        ResultSetResolver = resultSetResolver;
        MergingEngine = new UnionByOrdinalEngine(resultSetResolver);
        Variables = variables;
        VariableName = variableName;
    }

    public IResultSet Execute()
    {
        var sequence = SequenceResolver.Execute();

        if (sequence.Count == 0)
            return new EmptyResultSetResolver(new EmptyResultSetResolverArgs(new LiteralScalarResolver<int>(0))).Execute();

        Variables.Add(VariableName, new InternalVariable(new LiteralScalarResolver<object>(sequence[0]!)));
        var rs = ResultSetResolver.Execute();
        sequence.RemoveAt(0);

        foreach (var item in sequence)
        {
            Variables[VariableName] = new InternalVariable(new LiteralScalarResolver<object>(item));
            rs = MergingEngine.Execute(rs);
        }

        return rs;
    }
}
