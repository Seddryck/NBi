using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Member;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public class SubsetOfConstraint : AbstractMembersCollectionConstraint
    {
        /// <summary>
        /// Construct a EquivalentToConstraint
        /// </summary>
        /// <param name="expected">The command to retrieve the list of expected items</param>
        public SubsetOfConstraint(IEnumerable<string> expected)
            : base(expected)
        {
        }

        /// <summary>
        /// Construct a EquivalentToConstraint
        /// </summary>
        /// <param name="expected">The list of expected items</param>
        public SubsetOfConstraint(IDbCommand expected)
            : base(expected)
        {
        }

        

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public new SubsetOfConstraint IgnoreCase
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
            return new CollectionSubsetConstraint(ExpectedItems.Select(str => StringComparerHelper.Build(str)).ToList());
        }

        protected override string GetPredicate()
        {
            return string.Format("all the {0} of \"{1}\" are strictly defined in following set:", GetFunctionLabel(Request.Function), Request.Path);
        }

    }
}
