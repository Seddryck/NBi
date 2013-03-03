using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure.Contains
{
    public class ContainsItemConstraint : NUnitCtr.CollectionContainsConstraint
    {
        protected string expectedCaption;
        protected string expectedDisplayFolder;

        public MetadataDiscoveryRequest Request { get; set; }
        public IComparer Comparer { get; set; }

        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainsItemConstraint(string expected)
            : base(StringComparerHelper.Build(expected))
        {
            expectedCaption = expected;
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
            var filterExpression = description.GetFilterExpression(Request.GetAllFilters());
            var nextTargetExpression = description.GetNextTargetExpression(Request.Target);
            var expectationExpression = expectedCaption;

            writer.WritePredicate(string.Format("find a {0} named '{1}' contained {2}",
                nextTargetExpression,
                expectationExpression,
                filterExpression));
        }

    }
}
