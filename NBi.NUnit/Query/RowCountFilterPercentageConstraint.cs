using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Core.ResultSet.Filtering;

namespace NBi.NUnit.Query
{
    public class RowCountFilterPercentageConstraint : RowCountFilterConstraint
    {
        public RowCountFilterPercentageConstraint(DifferedConstraint differed, IResultSetFilter filter)
            : base(differed, filter)
        { }

        protected override bool doMatch(int actual)
        {
            var percentage = (decimal)actual / actualResultSet.Rows.Count() * 100;
            countResult = differed.Resolve().ApplyTo(percentage);
            return countResult.IsSuccess;
        }

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            writer.Write("percentage of rows matching the predicate is " + countResult?.Description.TrimEnd('m') + "%");
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            writer.WriteActualValue($"{countResult?.ActualValue:G29}%");
        }
    }
}