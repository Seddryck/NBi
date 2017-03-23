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
    public class PredicateFilter : IResultSetFilter
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

        public ResultSet.ResultSet AntiApply(ResultSet.ResultSet rs)
        {
            return Apply(rs, (x => !x));
        }

        public ResultSet.ResultSet Apply(ResultSet.ResultSet rs)
        {
            return Apply(rs, (x => x));
        }

        protected ResultSet.ResultSet Apply(ResultSet.ResultSet rs, Func<bool,bool> onApply)
        {
            var filteredRs = new ResultSet.ResultSet();
            var table = rs.Table.Clone();
            filteredRs.Load(table);
            

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
                if (onApply(predicate.Apply(value)))
                    filteredRs.Table.ImportRow(row);
            }

            filteredRs.Table.AcceptChanges();
            return filteredRs;
        }
    }
}
