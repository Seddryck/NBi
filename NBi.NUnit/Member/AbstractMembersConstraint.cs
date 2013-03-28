using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Member
{
    public abstract class AbstractMembersConstraint : NUnitCtr.Constraint
    {
        private MembersAdomdEngine commandFactory;
        protected virtual NUnitCtr.Constraint InternalConstraint {get; set;}
        
        public IComparer Comparer { get; set; }
        
        /// <summary>
        /// Request for metadata extraction
        /// </summary>
        public MembersDiscoveryRequest Request { get; protected set; }

        /// <summary>
        /// Engine dedicated to MetadataExtractor acquisition
        /// </summary>
        protected internal MembersAdomdEngine CommandFactory
        {
            get
            {
                if (commandFactory == null)
                    commandFactory = new MembersAdomdEngine();
                return commandFactory;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                commandFactory = value;
            }
        }

        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public AbstractMembersConstraint()
        {
            Comparer = new NBi.Core.Analysis.Member.Member.ComparerByCaption(true);
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        protected void IgnoreCase()
        {
            Comparer = new NBi.Core.Analysis.Member.Member.ComparerByCaption(false);
        }

        #endregion
        
        #region Specific NUnit
        public override bool Matches(object actual)
        {
            if (actual is MembersDiscoveryRequest)
                return Process((MembersDiscoveryRequest)actual);
            else if (actual is MemberResult)
            {
                this.actual = actual;
                var ctr = InternalConstraint;
                if (ctr is NUnitCtr.CollectionItemsEqualConstraint)
                    ctr = ((NUnitCtr.CollectionItemsEqualConstraint)ctr).Using(Comparer);
                var res = ctr.Matches(actual);
                return res;
            }
            else
                throw new ArgumentException();
        }

        protected bool Process(MembersDiscoveryRequest actual)
        {
            Request = actual;
            var engine = CommandFactory;
            MemberResult result = engine.GetMembers(Request);
            return this.Matches(result);
        }

        #endregion

    }
}
