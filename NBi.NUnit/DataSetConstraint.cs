using System.Data.SqlClient;
using NBi.Core.Database;
using NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class DataSetConstraint: Constraint
    {
        protected IDataSetComparer _engine;

        public DataSetConstraint(string referenceConnectionString, string actualConnectionString)
        {
            _engine = new DataSetComparer(referenceConnectionString, actualConnectionString);
        }

        public DataSetConstraint(string referenceConnectionString, string referenceSql, string actualConnectionString)
        {
            _engine = new DataSetComparer(referenceConnectionString, referenceSql, actualConnectionString);
        }

        public DataSetConstraint(IDataSetComparer engine)
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

        public virtual bool Matches(string actual)
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
