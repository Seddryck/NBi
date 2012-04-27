using NBi.Core.Database;

namespace NBi.NUnit
{
    public class DataSetContentConstraint : DataSetConstraint
    {
        public DataSetContentConstraint(string referenceConnectionString, string actualConnectionString)
            : base(referenceConnectionString, actualConnectionString)
        {
        }

        public DataSetContentConstraint(string referenceConnectionString, string referenceSql, string actualConnectionString)
            : base(referenceConnectionString, referenceSql, actualConnectionString)
        {
        }


        public DataSetContentConstraint(IDataSetComparer engine)
            : base(engine)
        {
        }

        public override bool Matches(string actual)
        {
            var res = _engine.ValidateContent(actual);
            return res.ToBoolean();
        }
    }
}
