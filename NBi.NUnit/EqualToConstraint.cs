using NBi.Core;
using NBi.Core.Analysis.Query;
using NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class EqualToConstraint : Constraint
    {
        /// <summary>
        /// Engine dedicated to result set comparaison
        /// </summary>
        protected IResultSetComparer _engine;

        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected Result _res;

        public EqualToConstraint(string connectionString, string expectedResultSetPath)
        {
            _engine = new ResultSetComparer(connectionString, expectedResultSetPath);
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
        /// Handle a sql string or a sqlCommand and check it with the engine
        /// </summary>
        /// <param name="actual">MDX Command</param>
        /// <returns>true, if the result of query execution is exactly identical to the content of the resultset</returns>
        public override bool Matches(object actual)
        {
            if (actual.GetType() == typeof(string))
                return Matches((string)actual);
            else
                return false;

        }

        /// <summary>
        /// Handle a sql string and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string</param>
        /// <returns>true, if the query defined in parameter is executed in less that expected else false</returns>
        public bool Matches(string actual)
        {
            _res = _engine.Validate(actual);
            return _res.ToBoolean();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
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
