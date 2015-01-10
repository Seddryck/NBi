﻿using System;
using System.Collections;
using System.Linq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework;

namespace NBi.NUnit.Member
{
    public class CountConstraint : AbstractMembersConstraint
    {
        private int? exactly;
        private int? moreThan;
        private int? lessThan;

        /// <summary>
        /// .ctor, define the default engine used by this constraint
        /// </summary>
        public CountConstraint()
        {
        }

        protected override NUnitCtr.Constraint BuildInternalConstraint()
        {
            NUnitCtr.Constraint ctr = null;

            if (exactly.HasValue)
            {
                if (ctr != null)
                    ctr = ctr.And.EqualTo(exactly.Value);
                else
                    ctr = new NUnitCtr.EqualConstraint(exactly.Value);
            }

            if (moreThan.HasValue)
            {
                if (ctr != null)
                    ctr = ctr.And.GreaterThan(moreThan.Value);
                else
                    ctr = new NUnitCtr.GreaterThanConstraint(moreThan.Value);
            }

            if (lessThan.HasValue)
            {
                if (ctr != null)
                    ctr = ctr.And.LessThan(lessThan.Value);
                else
                    ctr = new NUnitCtr.LessThanConstraint(lessThan.Value);
            }

            return ctr;
        }

        public CountConstraint Exactly(int value)
        {
            exactly = value;
            return this;
        }

        public CountConstraint MoreThan(int value)
        {
            moreThan = value;
            return this;
        }

        public CountConstraint LessThan(int value)
        {
            lessThan = value;
            return this;
        }

        protected override bool DoMatch(NUnitCtr.Constraint ctr)
        {
            IResolveConstraint exp = ctr;
            var multipleConstraint = exp.Resolve();
            return multipleConstraint.Matches(((MemberResult)actual).Count);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate(string.Format("On perspective \"{0}\", the {1} of \"{2}\" are "
                                                            , Request.Perspective
                                                            , Request.Function.ToLower()
                                                            , Request.Path));
            if (exactly.HasValue)
            {
                writer.WritePredicate("exactly");
                writer.WriteExpectedValue(exactly.Value);
                return;
            }

            if (moreThan.HasValue && lessThan.HasValue)
            {
                writer.WritePredicate("between");
                writer.WriteExpectedValue(moreThan.Value);
                writer.WriteConnector("and");
                writer.WriteExpectedValue(lessThan.Value); 
                return;
            }

            if (moreThan.HasValue)
            {
                writer.WritePredicate("more than");
                writer.WriteExpectedValue(moreThan.Value);
                return;
            }

            if (lessThan.HasValue)
            {
                writer.WritePredicate("less than");
                writer.WriteExpectedValue(lessThan.Value);
                return;
            }

            writer.WriteActualValue(((ICollection)actual).Count);
        }
    }
}
