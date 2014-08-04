using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Query
{
    public class EvaluateRowsConstraint : NUnitCtr.Constraint
    {
        
        
        private readonly IEnumerable<IColumnVariable> variables;
        private readonly IEnumerable<IColumnExpression> expressions;

        protected ResultSet actualResultSet;
        protected List<RowEvaluationResult> evaluationResults;

        /// <summary>
        /// Engine dedicated to ResultSet acquisition
        /// </summary>
        protected IResultSetBuilder resultSetBuilder;
        protected internal IResultSetBuilder ResultSetBuilder
        {
            get
            {
                if(resultSetBuilder==null)
                    resultSetBuilder = new ResultSetBuilder();
                return resultSetBuilder;
            }
            set
            {
                if(value==null)
                    throw new ArgumentNullException();
                resultSetBuilder = value;
            }
        }
        
        public EvaluateRowsConstraint (IEnumerable<IColumnVariable> variables, IEnumerable<IColumnExpression> expressions)
        {
            this.variables = variables;
            this.expressions = expressions;
        }

        /// <summary>
        /// Handle an IDbCommand and compare it to a predefined resultset
        /// </summary>
        /// <param name="actual">An OleDbCommand, SqlCommand or AdomdCommand</param>
        /// <returns>true, if the result of query execution is exactly identical to the content of the resultset</returns>
        public override bool Matches(object actual)
        {
            if (actual is IDbCommand)
                return Process((IDbCommand)actual);
            else if (actual is ResultSet)
                return doMatch((ResultSet)actual);
            else
                return false;

        }

        protected bool doMatch(ResultSet actual)
        {
            this.actualResultSet = actual;

            var validationEngine = new RowValidator();
            evaluationResults = new List<RowEvaluationResult>();
            int rowIndex=0;
            foreach (DataRow row in actualResultSet.Rows)
	        {
		        var valuedVariables = new Dictionary<string, Object>();
                foreach (var v in variables)
	                valuedVariables.Add(v.Name, row[v.Column]);

                var valuedExpressions = new List<ValuedExpression>();
                foreach (var e in expressions)
                    valuedExpressions.Add(new ValuedExpression(e.Value, row[e.Column], e.Type, e.Tolerance));

                evaluationResults.Add(new RowEvaluationResult(rowIndex, valuedVariables, validationEngine.Execute(valuedVariables, valuedExpressions)));
                rowIndex += 1;
	        }
            bool value = evaluationResults.Aggregate<RowEvaluationResult, bool>(true, (total, r) => total && (r.CountExpressionValidationFailed()==0));
            return value;
        }

        /// <summary>
        /// Handle an IDbCommand (Query and ConnectionString) and check it with the expectation (Another IDbCommand or a ResultSet)
        /// </summary>
        /// <param name="actual">IDbCommand</param>
        /// <returns></returns>
        public bool Process(IDbCommand actual)
        {
            ResultSet rsActual = GetResultSet(actual);
            return this.Matches(rsActual);
        }

        protected ResultSet GetResultSet(Object obj)
        {
            return ResultSetBuilder.Build(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            FormatResultSet(writer, actualResultSet);
        }

        public override void WriteMessageTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate("The result of the query's execution doesn't validate the list of expressions.");
            var totalFailed = evaluationResults.Aggregate<RowEvaluationResult, int>(0, (total, r) => total += r.CountExpressionValidationFailed());
            if (totalFailed>1)
                writer.WriteLine("{0} expressions are not matching with the expected result.", totalFailed);
            else
                writer.WriteLine("One expression is not matching with the expected result.");

            base.WriteMessageTo(writer);
            DisplayDifferences(writer, evaluationResults);
        }

        protected void DisplayDifferences(NUnitCtr.MessageWriter writer, IEnumerable<RowEvaluationResult> results)
        {
            var failedRows = results.Where(r => r.CountExpressionValidationFailed() > 0);
            if (failedRows.Count()>1)
                writer.WriteLine("{0} of the {1} rows don't validate at least one expression.", failedRows.Count(), results.Count());
            else
                writer.WriteLine("{0} of the {1} rows doesn't validate at least one expression.", failedRows.Count(), results.Count());

            var failedRowsSample = failedRows.Take(10);
            foreach (var failedRow in failedRowsSample)
            {               
                writer.WriteLine();
                writer.WriteLine("Row {0}: ", failedRow.RowIndex);
                foreach (var failedExpression in failedRow.Results.Where(exp => !exp.IsValid))
                {
                    writer.WriteLine("    The expression '{0}' is not validated.", failedExpression.Sentence);
                    writer.WriteLine("    The expected result was '{0}' but the actual value is '{1}'", failedExpression.Expected, failedExpression.Actual);
                }

                foreach (var variable in failedRow.ValuedVariables)
                    writer.WriteLine("    Variable '{0}' had value of '{1}'", variable.Key, variable.Value);
            }
            writer.WriteLine();
            if (failedRowsSample.Count()<failedRows.Count())
                writer.WriteLine("... {0} of {1} failing rows skipped for display purpose.", failedRows.Count()-failedRowsSample.Count(), failedRows.Count());
        }

             

        protected virtual void FormatResultSet(NUnitCtr.MessageWriter writer, ResultSetCompareResult.Sample sample, bool compare)
        {
            var textCreator = new ResultSetTextWriter();
            var output = textCreator.BuildContent(sample.Rows, sample.Count, compare);
            foreach (var line in output)
                writer.WriteLine(line);                
        }


        protected virtual void FormatResultSet(NUnitCtr.MessageWriter writer, ResultSet resultSet)
        {
            var rows = resultSet.Rows.Cast<DataRow>().ToList();
            var textCreator = new ResultSetTextWriter();
            var output = textCreator.BuildContent(rows, rows.Count(), false);
            foreach (var line in output)
                writer.WriteLine(line);
        }
    }
}
