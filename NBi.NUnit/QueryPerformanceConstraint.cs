using System.Data.SqlClient;
using NBi.Core.Database;
using NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class QueryPerformanceConstraint : Constraint
    {
        protected IQueryPerformance _engine;

        public QueryPerformanceConstraint(string connectionString, int maxTimeMilliSeconds)
        {
            _engine = new QueryPerformance(connectionString, maxTimeMilliSeconds);
        }

        public QueryPerformanceConstraint(IQueryPerformance engine)
        {
            _engine = engine;
        }

        public override bool Matches(object actual)
        {
            if (actual.GetType() == typeof(string))
                return Matches((string)actual);
            else if (actual.GetType() == typeof(SqlCommand))
                return Matches(((SqlCommand)actual).CommandText);
            else
                return false;

        }

        public bool Matches(string actual)
        {
            var res = _engine.Validate(actual);
            return res.ToBoolean();
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("Date is not null");
            writer.WriteExpectedValue("");
        }
    }
}