using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    class PredicateFilter : IResultSetFilter
    {
        private readonly IEnumerable<IColumnExpression> expressions;
        private readonly IEnumerable<IColumnVariable> variables;
        private readonly IPredicateInfo predicateInfo;

        public PredicateFilter(IEnumerable<IColumnVariable> variables, IEnumerable<IColumnExpression> expressions, IPredicateInfo predicateInfo)
        {
            this.variables = variables;
            this.expressions = expressions;
            this.predicateInfo = predicateInfo;
        }

        public ResultSet.ResultSet Apply(ResultSet.ResultSet rs)
        {
            var filteredRs = new ResultSet.ResultSet();
            var factory = new PredicateFactory();
            var predicate = factory.Get(predicateInfo);

            foreach (DataRow row in rs.Rows)
            {
                var dico = new Dictionary<string, object>();
                foreach (var variable in variables)
	                dico.Add(variable.Name, row[variable.Column]);

                foreach (var expression in expressions)
                {
                    var exp = new NCalc.Expression(expression.Value);
                    exp.Parameters = dico;
                    var result = exp.Evaluate();
                    dico.Add(expression.Name, result);
                }

                var value = dico[predicateInfo.Name];
                if (predicate.Compare(value, predicateInfo.Reference))
                    filteredRs.Table.Rows.Add(row);
            }

            return filteredRs;
        }
    }
}
