using System;
using System.Collections;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Core.Analysis;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Metadata;

namespace NBi.NUnit.Member
{
    public class CountConstraint : NUnitCtr.Constraint
    {
        int? exactly { get; set; }
        int? moreThan { get; set; }
        int? lessThan { get; set; }

        private NUnitCtr.Constraint internalConstraint;

        protected DiscoverCommand command;
        protected IDiscoverMemberEngine memberEngine;

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
        /// .ctor, define the default engine used by this constraint
        /// </summary>
        public CountConstraint()
        {
        }


        public CountConstraint Exactly(int value)
        {
            if (internalConstraint != null)
                internalConstraint = internalConstraint.And.EqualTo(value);
            else
                internalConstraint = new NUnitCtr.EqualConstraint(value);
            return this;
        }

        public CountConstraint MoreThan(int value)
        {
            if (internalConstraint != null)
                internalConstraint = internalConstraint.And.GreaterThan(value);
            else
                internalConstraint = new NUnitCtr.GreaterThanConstraint(value);
            return this;
        }

        public CountConstraint LessThan(int value)
        {
            if (internalConstraint != null)
                internalConstraint = internalConstraint.And.LessThan(value);
            else
                internalConstraint = new NUnitCtr.LessThanConstraint(value);
            return this;
        }


        //public CountConstraint Approximatively(int value, int tolerance)
        //{
        //    if (internalConstraint != null)
        //        internalConstraint = internalConstraint.And.T(value);
        //    else
        //        internalConstraint = new NUnitCtr.
        //    return this;
        //}

        public override bool Matches(object actual)
        {
            if (actual is DiscoverCommand)
                return Process((DiscoverCommand)actual);
            if (actual is ICollection)
                return Matches((ICollection)actual);

            return false;
        }

        protected bool Process(DiscoverCommand actual)
        {
            command = actual;
            var extr = GetEngine();
            MemberResult result = extr.Execute(command);
            return this.Matches(result);
        }

        /// <summary>
        /// Handle a ICollection and check it directly
        /// </summary>
        /// <param name="actual">an ICollection</param>
        /// <returns></returns>
        public bool Matches(ICollection actual)
        {
            if (internalConstraint == null)
                return false;

            return internalConstraint.Matches(actual.Count);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (exactly.HasValue)
            {
                writer.WritePredicate("exactly");
                writer.WriteExpectedValue(exactly.Value);
                return;
            }

            if (moreThan.HasValue && lessThan.HasValue)
            {
                writer.WritePredicate("between");
                writer.WriteExpectedValue(lessThan.Value);
                writer.WriteConnector("and");
                writer.WriteExpectedValue(moreThan.Value);
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
        }
    }
}
