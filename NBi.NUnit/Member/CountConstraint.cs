using System;
using System.Collections;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public class CountConstraint : NUnitCtr.Constraint
    {
        private int? exactly { get; set; }
        private int? moreThan { get; set; }
        private int? lessThan { get; set; }
        private ICollection actualCollection 
        { 
            get 
            {
                return (ICollection)actual;
            }
            set
            {
                actual = value;
            }
        }
                

        private NUnitCtr.Constraint internalConstraint;

        protected MembersDiscoveryRequest request;
        protected MembersAdomdEngine memberEngine;

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

            exactly = value;
            return this;
        }

        public CountConstraint MoreThan(int value)
        {
            if (internalConstraint != null)
                internalConstraint = internalConstraint.And.GreaterThan(value);
            else
                internalConstraint = new NUnitCtr.GreaterThanConstraint(value);

            moreThan = value;
            return this;
        }

        public CountConstraint LessThan(int value)
        {
            if (internalConstraint != null)
                internalConstraint = internalConstraint.And.LessThan(value);
            else
                internalConstraint = new NUnitCtr.LessThanConstraint(value);

            lessThan = value;
            return this;
        }

        public override bool Matches(object actual)
        {
            if (actual is MembersDiscoveryRequest)
                return Process((MembersDiscoveryRequest)actual);
            if (actual is ICollection)
                return Matches((ICollection)actual);

            return false;
        }

        protected bool Process(MembersDiscoveryRequest actual)
        {
            request = actual;
            var extr = GetEngine();
            MemberResult result = extr.GetMembers(request);
            return this.Matches(result);
        }

        /// <summary>
        /// Handle a ICollection and check it directly
        /// </summary>
        /// <param name="actual">an ICollection</param>
        /// <returns></returns>
        public bool Matches(ICollection actual)
        {
            actualCollection = actual;
            
            if (internalConstraint == null)
                return false;

            IResolveConstraint exp = internalConstraint;
            var multipleConstraint = exp.Resolve();

            return multipleConstraint.Matches(actual.Count);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            writer.WritePredicate(string.Format("On perspective \"{0}\", the {1} of \"{2}\" are "
                                                            , request.Perspective
                                                            , request.Function.ToLower()
                                                            , request.Path));
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

            writer.WriteActualValue(actualCollection.Count);
        }
    }
}
