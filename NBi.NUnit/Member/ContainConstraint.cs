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

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (Request != null)
            {
                writer.WritePredicate(string.Format("On perspective \"{0}\", a {1} of \"{2}\" containing a member with caption"
                                                            , Request.Perspective
                                                            , GetFunctionLabel(Request.Function)
                                                            , Request.Path));
                writer.WriteExpectedValue(ExpectedItems);
            }
            
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (actual is MemberResult && ((MemberResult)actual).Count() > 0 && ((MemberResult)actual).Count()<=15)
                writer.WriteActualValue((IEnumerable)actual);
            else if (actual is MemberResult && ((MemberResult)actual).Count() > 0 && ((MemberResult)actual).Count() > 15)
            {
                writer.WriteActualValue(((IEnumerable<NBi.Core.Analysis.Member.Member>)actual).Take(10));
                writer.WriteActualValue(string.Format(" ... and {0} others.", ((MemberResult)actual).Count() - 10));
            }
            else
                writer.WriteActualValue(new NothingFoundMessage());
        }

    }
}
