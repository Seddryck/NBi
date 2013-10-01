using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Member;
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

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if (Request != null)
            {
                writer.WritePredicate(string.Format("On perspective \"{0}\", a {1} of \"{2}\" the members are identical to the predefined set"
                                                            , Request.Perspective
                                                            , GetFunctionLabel(Request.Function)
                                                            , Request.Path));
                writer.WriteExpectedValue(ExpectedItems);
            }
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (actual is MemberResult && ((MemberResult)actual).Count() > 0 && ((MemberResult)actual).Count() <= 15)
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
