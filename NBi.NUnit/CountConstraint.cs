using System.Collections;
using System.Xml.Schema;
using NBi.Core;
using NBi.Core.Analysis;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class CountConstraint : NUnitCtr.Constraint
    {
        int? exactly { get; set; }
        int? moreThan { get; set; }
        int? lessThan { get; set; }

        
        /// <summary>
        /// .ctor, define the default engine used by this constraint
        /// </summary>
        public CountConstraint()
        {
        }


        public CountConstraint Exactly(int value)
        {
            this.exactly = value; 
            return this;
        }

        public CountConstraint MoreThan(int value)
        {
            this.moreThan = value;
            return this;
        }

        public CountConstraint LessThan(int value)
        {
            this.lessThan = value; 
            return this;
        }

        public override bool Matches(object actual)
        {
            if (actual is ICollection)
                return Matches((ICollection)actual);

            return false;
        }

        /// <summary>
        /// Handle a ICollection and check it with the engine
        /// </summary>
        /// <param name="actual">an ICollection</param>
        /// <returns></returns>
        public bool Matches(ICollection actual)
        {
            if (!(moreThan.HasValue || lessThan.HasValue || exactly.HasValue))
                return false;
            if (moreThan.HasValue && actual.Count <= moreThan.Value)
                return false;
            if (lessThan.HasValue && actual.Count >= lessThan.Value)
                return false;
            if (exactly.HasValue && actual.Count != exactly.Value)
                return false;

            return true;
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (exactly.HasValue)
            {
                writer.WritePredicate("exactly");
                writer.WriteExpectedValue(exactly.Value);
                return;
            }

            if (moreThan.HasValue && lessThan.HasValue)
            {
                writer.WritePredicate("between");
                writer.WriteExpectedValue(lessThan.Value);
                writer.WriteConnector("and");
                writer.WriteExpectedValue(moreThan.Value);
                return;
            }

            if (moreThan.HasValue)
            {
                writer.WritePredicate("more than");
                writer.WriteExpectedValue(moreThan.Value);
                return;
            }

            if (lessThan.HasValue)
            {
                writer.WritePredicate("less than");
                writer.WriteExpectedValue(lessThan.Value);
                return;
            }  
        }
    }
}
