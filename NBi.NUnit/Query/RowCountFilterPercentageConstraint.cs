using System;
using NBi.Core;
using NUnitCtr = NUnit.Framework.Constraints;
using NUnitFwk = NUnit.Framework;
using NBi.Core.ResultSet;
using System.Data;
using NBi.Core.Calculation;
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
            this.actual = Convert.ToDecimal(actual) / actualResultSet.Rows.Count * 100;
            ctr = differed.Resolve();
            return ctr.Matches(this.actual);
        }
       
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate("percentage of rows matching the predicate is " + TransformDecimalToPercentage(ctr.WriteDescriptionTo));
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            writer.WriteActualValue(TransformDecimalToPercentage(ctr.WriteActualValueTo));
        }

        protected string TransformDecimalToPercentage(Action<NUnitFwk.TextMessageWriter> action)
        {
            var sb = new System.Text.StringBuilder();
            var localWriter = new NUnitFwk.TextMessageWriter();
            action(localWriter);
            var childMessage = localWriter.ToString();
            sb.Append(childMessage.Substring(0, childMessage.LastIndexOf(" ") + 1));
            sb.Append(decimal.Parse(childMessage.Substring(childMessage.LastIndexOf(" ") + 1).Replace("m", ""), System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat));
            sb.Append("%");

            return sb.ToString();
        }

    }
}