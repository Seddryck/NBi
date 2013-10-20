using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public class EquivalentToConstraint : AbstractMembersCollectionConstraint
    {
        /// <summary>
        /// Construct a EquivalentToConstraint
        /// </summary>
        /// <param name="expected">The command to retrieve the list of expected items</param>
        public EquivalentToConstraint(IEnumerable<string> expected)
            : base(expected)
        {
        }

        /// <summary>
        /// Construct a EquivalentToConstraint
        /// </summary>
        /// <param name="expected">The list of expected items</param>
        public EquivalentToConstraint(IDbCommand expected)
            : base(expected)
        {
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public new EquivalentToConstraint IgnoreCase
        {
            get
            {
                base.IgnoreCase();
                return this;
            }
        }

        #endregion

        protected override NUnitCtr.Constraint BuildInternalConstraint()
        {
            return new CollectionEquivalentConstraint(ExpectedItems.Select(str => StringComparerHelper.Build(str)).ToList());
        }

        protected override string GetPredicate()
        {
            return string.Format("the {0} of \"{1}\" are strictly equivalent to the following set:", GetFunctionLabel(Request.Function), Request.Path);
        }


        protected override ListComparer.Comparison GetComparisonType()
        {
            return ListComparer.Comparison.Both;
        }
        
    }
}
