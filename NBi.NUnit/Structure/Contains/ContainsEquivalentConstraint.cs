using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using NBi.Core;
using NBi.Core.Analysis.Metadata;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure.Contains
{
    public class ContainsEquivalentConstraint : NUnitCtr.CollectionEquivalentConstraint
    {
        protected ContainsConstraint ParentConstraint { get; set; }
        public IComparer Comparer { get; set; }
        
        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainsEquivalentConstraint(IEnumerable<string> expected, ContainsConstraint parentConstraint)
            : base(expected.Select(str => StringComparerHelper.Build(str)))
        {
            this.ParentConstraint = parentConstraint;
        }

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if (ParentConstraint.Request != null)
            {
                var description = new DescriptionStructureHelper();
                var filterExpression = description.GetFilterExpression(ParentConstraint.Request.GetAllFilters());
                var nextTargetExpression = description.GetNextTargetPluralExpression(ParentConstraint.Request.Target);
                var expectationExpression = new StringBuilder();
                foreach (string item in (IEnumerable<string>)(ParentConstraint.Expected))
                    expectationExpression.AppendFormat("< {0} >, ", item);
                expectationExpression.Remove(expectationExpression.Length - 2, 2);

                writer.WritePredicate(string.Format("find the exact list of {0} named '{1}' contained {2}",
                    nextTargetExpression,
                    expectationExpression,
                    filterExpression));

            }
            else
                base.WriteDescriptionTo(writer);
        }

        public override void WriteActualValueTo(MessageWriter writer)
        {
            if (actual is IEnumerable<IField> && ((IEnumerable<IField>)actual).Count() > 0)
                base.WriteActualValueTo(writer);
            else
                writer.WriteActualValue(new WriterHelper.NothingFoundMessage());
        }

    }
}
