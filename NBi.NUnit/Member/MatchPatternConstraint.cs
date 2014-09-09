using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public class MatchPatternConstraint : AbstractMembersConstraint
    {
        private string regex;
        private IList<string> invalidMembers = new List<string>();

        /// <summary>
        /// .ctor, define the default engine used by this constraint
        /// </summary>
        public MatchPatternConstraint()
            : base()
        {
        }

        protected override NUnitCtr.Constraint BuildInternalConstraint()
        {
            NUnitCtr.Constraint ctr = null;

            if (!string.IsNullOrEmpty(regex))
            {
                if (ctr != null)
                    ctr = ctr.And.Matches(regex);
                else
                    ctr = new NUnitCtr.RegexConstraint(regex);
            }

            return ctr;
        }

        #region Modifiers
        /// <summary>
        /// Set the regex pattern
        /// </summary>
        public MatchPatternConstraint Regex(string pattern)
        {
            this.regex = pattern;
            return this;
        }
        #endregion

        protected override bool DoMatch(NUnitCtr.Constraint ctr)
        {
            throw new InvalidOperationException();
        }

        protected virtual bool DoMatch(NUnitCtr.Constraint ctr, string member)
        {
            IResolveConstraint exp = ctr;
            var multipleConstraint = exp.Resolve();
            return multipleConstraint.Matches(member);
        }

        public override bool Matches(object actual)
        {
            if (actual is MembersDiscoveryRequest)
                return Process((MembersDiscoveryRequest)actual);
            else if (actual is MemberResult)
            {   
                this.actual = actual;

                var res = true;
                foreach (var member in (MemberResult)actual)
                {
                    var ctr = BuildInternalConstraint();
                    if (!DoMatch(ctr, member.Caption))
                    {
                        res = false;
                        invalidMembers.Add(member.Caption);
                    }
                }
                return res;
            }
            else
                throw new ArgumentException();
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            //writer.WriteActualValue(actual);
            
            writer.WritePredicate(string.Format("On perspective \"{0}\", the {1} of \"{2}\" match the"
                                                            , Request.Perspective
                                                            , Request.Function.ToLower()
                                                            , Request.Path));
            if (!string.IsNullOrEmpty(regex))
            {
                writer.WritePredicate("regex pattern");
                writer.WritePredicate(regex);
            }           
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (invalidMembers.Count == 1)
                writer.WriteMessageLine(string.Format("The element <{0}> doesn't validate this pattern", invalidMembers[0]));
            else
            {
                writer.WriteMessageLine(string.Format("{0} elements don't validate this pattern:", invalidMembers.Count));
                foreach (var invalidMember in invalidMembers)
                    writer.WriteMessageLine(string.Format("    <{0}>", invalidMember));
            }
        }
    }
}
