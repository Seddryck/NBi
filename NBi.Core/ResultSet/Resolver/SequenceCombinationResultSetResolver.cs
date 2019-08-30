using NBi.Core.ResultSet.Combination;
using NBi.Core.Sequence.Resolver;
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

        public ResultSet Execute()
        {
            if (Args.Resolvers.Count() == 0)
                throw new InvalidOperationException();

            var rs = Initialize(Args.Resolvers.First());
            foreach (var resolver in Args.Resolvers.Skip(1))
            {
                var cartesianProduct = new CartesianProductSequenceCombination(resolver);
                cartesianProduct.Execute(rs);
            }
            return rs;
        }
        
        private ResultSet Initialize(ISequenceResolver resolver)
        {
            var dataTable = new DataTable();
            var newColumn = new DataColumn($"Column{dataTable.Columns.Count}", typeof(object));
            dataTable.Columns.Add(newColumn);
            var sequence = resolver.Execute();
            foreach (var item in sequence)
            {
                var newRow = dataTable.NewRow();
                newRow[newColumn] = item;
                dataTable.Rows.Add(newRow);
            }
            dataTable.AcceptChanges();
            var rs = new ResultSet();
            rs.Load(dataTable);
            return rs;
        }
    }
}
 