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
    public class ContainsConstraint : NUnitCtr.Constraint
    {
        protected IEnumerable<string> expectedCaptions;
        protected IComparer comparer;
        protected DiscoverCommand command;
        protected IDiscoverMemberEngine memberEngine;

        protected NUnitCtr.CollectionItemsEqualConstraint internalConstraint;

        /// <summary>
        /// Engine dedicated to MetadataExtractor acquisition
        /// </summary>
        protected internal IDiscoverMemberEngine MemberEngine
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                memberEngine = value;
            }
        }

        protected IDiscoverMemberEngine GetEngine()
        {
            if (memberEngine == null)
                memberEngine = new CubeDimensionAdomdEngine();
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
        public new ContainsConstraint IgnoreCase
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
            if (actual is DiscoverCommand)
                return Process((DiscoverCommand)actual);
            else
            {
                internalConstraint = internalConstraint.Using(comparer);
                var res = internalConstraint.Matches(actual);
                return res;
            }
        }

        protected bool Process(DiscoverCommand actual)
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
                var pathParser = PathParser.Build(command);
                var persp = !string.IsNullOrEmpty(command.Perspective) ? string.Format("On perspective \"{0}\", a", command.Perspective) : "A";
                writer.WritePredicate(string.Format("{0} {1} identified by \"{2}\" containing a member with caption"
                                                            , persp
                                                            , pathParser.Position.Current
                                                            , command.Path));
                writer.WriteExpectedValue(expectedCaptions);
            }
            //else
            //    base.WriteDescriptionTo(writer);
        }

       
    }
}
