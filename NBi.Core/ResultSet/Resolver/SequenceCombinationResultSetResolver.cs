using NBi.Core.ResultSet.Combination;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    class SequenceCombinationResultSetResolver : IResultSetResolver
    {
        private SequenceCombinationResultSetResolverArgs Args { get; }
        public SequenceCombinationResultSetResolver(SequenceCombinationResultSetResolverArgs args)
            => Args = args;

        public IResultSet Execute()
        {
            if (!Args.Resolvers.Any())
                throw new InvalidOperationException();

            var rs = Initialize(Args.Resolvers.First());
            foreach (var resolver in Args.Resolvers.Skip(1))
            {
                var cartesianProduct = new CartesianProductSequenceCombination(resolver);
                cartesianProduct.Execute(rs);
            }
            return rs;
        }
        
        private IResultSet Initialize(ISequenceResolver resolver)
        {
            var dataTable = new DataTable();
            var newColumn = new DataColumn($"Column0", typeof(object));
            dataTable.Columns.Add(newColumn);
            var sequence = resolver.Execute();
            foreach (var item in sequence)
            {
                var newRow = dataTable.NewRow();
                newRow[newColumn] = item;
                dataTable.Rows.Add(newRow);
            }
            dataTable.AcceptChanges();
            return new DataTableResultSet(dataTable);
        }
    }
}
 