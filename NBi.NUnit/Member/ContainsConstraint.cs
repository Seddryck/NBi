using System;
using System.Collections;
using System.Collections.Generic;
using NBi.Core;
using NBi.Core.Analysis;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Metadata;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public class ContainsConstraint : NUnitCtr.CollectionContainsConstraint
    {
        protected IEnumerable<string> expectedCaptions;
        protected IComparer comparer;
        protected AdomdMemberCommand command;
        protected IMemberEngine memberEngine;

        /// <summary>
        /// Engine dedicated to MetadataExtractor acquisition
        /// </summary>
        protected internal IMemberEngine MemberEngine
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                memberEngine = value;
            }
        }

        protected IMemberEngine GetEngine()
        {
            if (memberEngine == null)
                memberEngine = new MemberAdomdEngine();
            return memberEngine;
        }

        /// <summary>
        /// Construct a CollectionContainsConstraint specific for Members
        /// </summary>
        /// <param name="expected"></param>
        public ContainsConstraint(string expected)
            : base(StringComparerHelper.Build(expected))
        {
            var list = new List<string>();
            list.Add(expected);
            expectedCaptions = list;
            comparer = new NBi.Core.Analysis.Member.Member.ComparerByCaption(true);
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
            if (actual is AdomdMemberCommand)
                return Process((AdomdMemberCommand)actual);
            else
            {
                base.Using(comparer);
                var res = base.Matches(actual);
                return res;
            }
        }

        public bool doMatch(IEnumerable<IElement> actual)
        {
            return base.Using(comparer).Matches(actual);
        }

        protected bool Process(AdomdMemberCommand actual)
        {
            command = actual;
            var extr = GetEngine();
            MemberResult result = extr.Execute(command);
            return this.Matches(result);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (command != null)
            {
                var pathParser = PathParser.Build(command.Path, command.Perspective);
                writer.WritePredicate(string.Format("On perspective \"{0}\", a {1} identified by \"{2}\" containing a member with caption"
                                                            , command.Perspective
                                                            , pathParser.Position.Current
                                                            , command.Path));
                writer.WriteExpectedValue(expectedCaptions);
            }
            else
                base.WriteDescriptionTo(writer);
        }

       
    }
}
