using System.Data.SqlClient;
using NBi.Core.Database;

namespace NBi.NUnit
{
    public class DataSetStructureConstraint : DataSetConstraint
    {
        public DataSetStructureConstraint(string referenceConnectionString, string actualConnectionString)
            : base(referenceConnectionString, actualConnectionString)
        {
        }

        public DataSetStructureConstraint(string referenceConnectionString, string referenceSql, string actualConnectionString)
            : base(referenceConnectionString, referenceSql, actualConnectionString)
        {
        }


        public DataSetStructureConstraint(IDataSetComparer engine) : base(engine)
        {
        }
        
        public override bool Matches(string actual)
        {
            var res = _engine.ValidateStructure(actual);
            return res.ToBoolean();
        }
    }
}
