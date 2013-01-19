using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public class ContainsConstraint : NUnitCtr.Constraint
    {
        protected IEnumerable<string> expectedCaptions;
        protected IComparer comparer;
        protected MembersDiscoveryRequest request;
        protected MembersAdomdEngine memberEngine;

        protected NUnitCtr.CollectionItemsEqualConstraint internalConstraint;
        /// <summary>
        /// Engine dedicated to MetadataExtractor acquisition
        /// </summary>
        protected internal MembersAdomdEngine MemberEngine
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                memberEngine = value;
            }
        }

        protected MembersAdomdEngine GetEngine()
        {
            if (memberEngine == null)
                memberEngine = new MembersAdomdEngine();
            return memberEngine;
        }

        /// <summary>
        /// Construct a CollectionContainsConstraint specific for Members
        /// </summary>
        /// <param name="expected"></param>
        public ContainsConstraint(string expected) : base()
        {
            var list = new List<string>();
            list.Add(expected);
            expectedCaptions = list;
            comparer = new NBi.Core.Analysis.Member.Member.ComparerByCaption(true);

            internalConstraint = new NUnitCtr.CollectionContainsConstraint(StringComparerHelper.Build(expected));
        }

        /// <summary>
        /// Construct a CollectionContainsConstraint specific for Members
        /// </summary>
        /// <param name="expected"></param>
        public ContainsConstraint(IEnumerable<string> expected)
            : base(expected)
        {
            expectedCaptions = expected;
            comparer = new NBi.Core.Analysis.Member.Member.ComparerByCaption(true);

            var expectedStringHelper = new List<StringComparerHelper>();
            foreach (var str in expected)
                expectedStringHelper.Add(StringComparerHelper.Build(str));

            internalConstraint = new NUnitCtr.CollectionSubsetConstraint(expectedStringHelper);
        }
        
        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public ContainsConstraint IgnoreCase
        {
            get
            {
                comparer = new NBi.Core.Analysis.Member.Member.ComparerByCaption(false);
                return this;
            }
        }
        #endregion


        public override bool Matches(object actual)
        {
            if (actual is MembersDiscoveryRequest)
                return Process((MembersDiscoveryRequest)actual);
            else
            {
                this.actual = actual;
                internalConstraint = internalConstraint.Using(comparer);
                var res = internalConstraint.Matches(actual);
                return res;
            }
        }

        protected bool Process(MembersDiscoveryRequest actual)
        {
            request = actual;
            var extr = GetEngine();
            MemberResult result = extr.GetMembers(request);
            return this.Matches(result);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (request != null)
            {
                writer.WritePredicate(string.Format("On perspective \"{0}\", a {1} of \"{2}\" containing a member with caption"
                                                            , request.Perspective
                                                            , GetFunctionLabel(request.Function)
                                                            , request.Path));
                writer.WriteExpectedValue(expectedCaptions);
            }
            //else
            //    base.WriteDescriptionTo(writer);
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
