using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Member;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public class SubsetOfConstraint : AbstractMembersConstraint
    {
        protected IEnumerable<string> Expected { get; set; }

        /// <summary>
        /// Construct a CollectionSubsetConstraint
        /// </summary>
        /// <param name="expected"></param>
        public SubsetOfConstraint(IEnumerable<string> expected)
            : base()
        {
            Expected = expected;
            InternalConstraint = new CollectionSubsetConstraint(expected.Select(str => StringComparerHelper.Build(str)).ToList());
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

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if (Request != null)
            {
                writer.WritePredicate(string.Format("On perspective \"{0}\", a {1} of \"{2}\" all members belong to a predefined set"
                                                            , Request.Perspective
                                                            , GetFunctionLabel(Request.Function)
                                                            , Request.Path));
                writer.WriteExpectedValue(Expected);
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

        protected string GetFunctionLabel(string function)
        {
            switch (function.ToLower())
            {
                case "children":
                    return "child";
                case "members":
                    return "member";
                default:
                    return "?";
            }
        }

        protected internal class NothingFoundMessage
        {
            public override string ToString()
            {
                return "nothing found";
            }
        }
    }
}
