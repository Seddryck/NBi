using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure.Contains
{
    public class ContainsEquivalentConstraint : NUnitCtr.CollectionEquivalentConstraint
    {
        public MetadataDiscoveryRequest Request { get; set; }
        public IComparer Comparer { get; set; }
        
        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainsEquivalentConstraint(IEnumerable<string> expected)
            : base(expected.Select(str => StringComparerHelper.Build(str)))
        {
        }

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if (Request != null)
            {
                var description = new DescriptionStructureHelper();
                var filterExpression = description.GetFilterExpression(Request.GetAllFilters());
                var nextTargetExpression = description.GetNextTargetExpression(Request.Target);
                var expectationExpression = "TODO";

                writer.WritePredicate(string.Format("find a list of {0} named '{1}' contained {2}",
                    nextTargetExpression,
                    expectationExpression,
                    filterExpression));

            }
            else
                base.WriteDescriptionTo(writer);
        }

    }
}
