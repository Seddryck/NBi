using System.Data.SqlClient;
using NBi.Core.Database;
using NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class QueryParserConstraint : Constraint
    {
        protected IQueryParser _engine;
        
        public QueryParserConstraint(string connectionString)
        {
            _engine = new QueryParser(connectionString);
        }

        public QueryParserConstraint(IQueryParser engine)
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
            var res= _engine.ValidateFormat(actual);
            return res.ToBoolean();
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("Date is not null");
            writer.WriteExpectedValue("");
        }
    }
}