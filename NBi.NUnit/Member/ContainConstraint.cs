using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public class ContainConstraint : AbstractMembersConstraint
    {
        protected IEnumerable<string> Expected {get; set;}
        
        /// <summary>
        /// Construct a CollectionContainsConstraint specific for Members
        /// </summary>
        /// <param name="expected"></param>
        public ContainConstraint(string expected) : base()
        {
            Expected = new List<string>() {expected};
        }

        /// <summary>
        /// Construct a CollectionContainsConstraint specific for Members
        /// </summary>
        /// <param name="expected"></param>
        public ContainConstraint(IEnumerable<string> expected)
            : base()
        {
            Expected = expected;
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
        
        public override bool Matches(object actual)
        {
            if (actual is MembersDiscoveryRequest)
                return Process((MembersDiscoveryRequest)actual);
            else if (actual is MemberResult)
            {
                this.actual = actual;
                NUnitCtr.Constraint ctr = null;
                foreach (var item in Expected)
                {
                    var localCtr = new NUnitCtr.CollectionContainsConstraint(StringComparerHelper.Build(item));
                    var usingCtr = localCtr.Using(Comparer);

                    if (ctr != null)
                        ctr = new NUnitCtr.AndConstraint(ctr, usingCtr);
                    else
                        ctr = usingCtr;
                }

                IResolveConstraint exp = ctr;
                var multipleConstraint = exp.Resolve();
                var res = multipleConstraint.Matches(actual);
                
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
            if (Request != null)
            {
                writer.WritePredicate(string.Format("On perspective \"{0}\", a {1} of \"{2}\" containing a member with caption"
                                                            , Request.Perspective
                                                            , GetFunctionLabel(Request.Function)
                                                            , Request.Path));
                writer.WriteExpectedValue(Expected);
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
