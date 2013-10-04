using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public class ContainConstraint : AbstractMembersCollectionConstraint
    {
        /// <summary>
        /// Construct a CollectionEquivalentConstraint
        /// </summary>
        /// <param name="expected">A unique expected member</param>
        public ContainConstraint(string expected)
            : this(new List<string>() {expected})
        {
        }
        
        /// <summary>
        /// Construct a EquivalentToConstraint
        /// </summary>
        /// <param name="expected">The command to retrieve the list of expected items</param>
        public ContainConstraint(IEnumerable<string> expected)
            : base(expected)
        {
        }

        /// <summary>
        /// Construct a EquivalentToConstraint
        /// </summary>
        /// <param name="expected">The list of expected items</param>
        public ContainConstraint(IDbCommand expected)
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

        protected override NUnitCtr.Constraint BuildInternalConstraint()
        {
            NUnitCtr.Constraint ctr = null;
            foreach (var item in ExpectedItems)
            {
                var localCtr = new NUnitCtr.CollectionContainsConstraint(StringComparerHelper.Build(item));
                var usingCtr = localCtr.Using(Comparer);

                if (ctr != null)
                    ctr = new NUnitCtr.AndConstraint(ctr, usingCtr);
                else
                    ctr = usingCtr;
            }
            return ctr;
        }

        protected override bool DoMatch(NUnitCtr.Constraint ctr)
        {
            IResolveConstraint exp = ctr;
            var multipleConstraint = exp.Resolve();
            return multipleConstraint.Matches(actual);
        }

        protected override string GetPredicate()
        {
            return string.Format("the {0} of \"{1}\" contain elements of the following set:", GetFunctionLabel(Request.Function), Request.Path);
        }

        protected override ListComparer.Comparison GetComparisonType()
        {
            return ListComparer.Comparison.MissingItems;
        }

    }
}
