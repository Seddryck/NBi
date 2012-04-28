using System.Data;
using System.Data.OleDb;
using NBi.Core;
using NBi.Core.Analysis.Query;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class EqualToConstraint : NUnitCtr.Constraint
    {
        /// <summary>
        /// Engine dedicated to result set comparaison
        /// </summary>
        protected IResultSetComparer _engine;

        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected Result _res;

        public EqualToConstraint(string expectedResultSetPath)
        {
            _engine = new ResultSetComparer(expectedResultSetPath);
        }

        /// <summary>
        /// .ctor mainly used for mocking
        /// </summary>
        /// <param name="engine">The engine to use</param>
        protected internal EqualToConstraint(IResultSetComparer engine)
        {
            _engine = engine;
        }

        /// <summary>
        /// Handle an IDbCommand and compare it to a predefined resultset
        /// </summary>
        /// <param name="actual">An OleDbCommand</param>
        /// <returns>true, if the result of query execution is exactly identical to the content of the resultset</returns>
        public override bool Matches(object actual)
        {
            if (actual.GetType() == typeof(OleDbCommand))
                return Matches((IDbCommand)actual);
            else
                return false;

        }

        /// <summary>
        /// Handle an IDbCommand (Query and ConnectionString) and check it with the engine
        /// </summary>
        /// <param name="actual">IDbCommand</param>
        /// <returns>true, if the query defined in parameter is executed in less that expected else false</returns>
        public bool Matches(IDbCommand actual)
        {
            _res = _engine.Validate(actual);
            return _res.ToBoolean();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Execution of the query doesn't match the expected result");
            foreach (var failure in _res.Failures)
            {
                sb.AppendLine(failure);
            }
            writer.WritePredicate(sb.ToString());
            //writer.WriteExpectedValue("");
        }
    }
}
