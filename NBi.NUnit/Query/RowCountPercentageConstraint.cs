using System;
using NBi.Core;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Core.ResultSet;
using System.Data;
using NBi.Core.Calculation;

namespace NBi.NUnit.Query
{
    public class RowCountPercentageConstraint : RowCountConstraint
    {
        public RowCountPercentageConstraint(NUnitCtr.Constraint childConstraint, IResultSetFilter filter)
            : base(childConstraint)
        {
            this.filter=filter;
        }

        protected override bool doMatch(int actual)
        {
            this.actual = Convert.ToDecimal(actual) / actualResultSet.Rows.Count;
            return child.Matches(this.actual);
        }
       
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate("ratio of rows after application of the filter and rows returned by execution of the query is");
            child.WriteDescriptionTo(writer);
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            child.WriteActualValueTo(writer);
        }
    }
}