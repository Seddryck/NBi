using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure
{
    public class ContainConstraint : AbstractCollectionConstraint
    {
        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainConstraint(string expected)
            : base(new List<string>() {expected})
        {
        }
        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainConstraint(IEnumerable<string> expected)
            : base(expected)
        {
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public new ContainConstraint IgnoreCase
        {
            get
            {
                base.IgnoreCase();
                return this;
            }
        }

        #endregion

        #region Specific NUnit
        public override bool Matches(object actual)
        {
            if (actual is MetadataDiscoveryRequest)
                return Process((MetadataDiscoveryRequest)actual);
            else
            {
                NUnitCtr.Constraint ctr = null;
                foreach (var item in Expected)
                {
                    var localCtr = new NUnitCtr.CollectionContainsConstraint(StringComparerHelper.Build(item));
                    var usingCtr = localCtr.Using(Comparer);

                    if (ctr != null)
                        ctr = new AndConstraint(ctr, usingCtr);
                    else
                        ctr = usingCtr;
                }

                IResolveConstraint exp = ctr;
                var multipleConstraint = exp.Resolve();
                var res = multipleConstraint.Matches(actual);
                
                return res;
            }
        }

        #endregion

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            var description = new DescriptionStructureHelper();
            var filterExpression = description.GetFilterExpression(Request.GetAllFilters());

            if (Expected.Count() == 1)
            {
                writer.WritePredicate(string.Format("find a {0} named '{1}' contained {2}",
                    description.GetTargetExpression(Request.Target),
                    Expected.First(),
                    filterExpression));
            }
            else
            {
                writer.WritePredicate(string.Format("find a list of {0} contained {1}",
                    description.GetTargetPluralExpression(Request.Target),
                    filterExpression));
            }
        }
    }
}
