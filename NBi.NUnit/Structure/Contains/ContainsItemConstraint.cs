using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Metadata;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure.Contains
{
    public class ContainsItemConstraint : NUnitCtr.CollectionContainsConstraint
    {
        protected ContainsConstraint ParentConstraint { get; set; }
        public IComparer Comparer { get; set; }

        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainsItemConstraint(string expected, ContainsConstraint parentConstraint)
            : base(StringComparerHelper.Build(expected))
        {
            this.ParentConstraint = parentConstraint;
            Comparer = new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(true);
            base.Using(Comparer);
        }
        
        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            var description = new DescriptionStructureHelper();
            var filterExpression = description.GetFilterExpression(ParentConstraint.Request.GetAllFilters());
            var nextTargetExpression = description.GetNextTargetExpression(ParentConstraint.Request.Target);
            var expectationExpression = (string)(ParentConstraint.Expected);

            writer.WritePredicate(string.Format("find a {0} named '{1}' contained {2}",
                nextTargetExpression,
                expectationExpression,
                filterExpression));
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
