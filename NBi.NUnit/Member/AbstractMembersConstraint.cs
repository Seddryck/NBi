using System;
using System.Collections;
using System.Linq;
using NBi.Core.Analysis.Member;
using NBi.Core.Analysis.Request;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework;


namespace NBi.NUnit.Member
{
    public abstract class AbstractMembersConstraint : NBiConstraint
    {
        private MembersAdomdEngine membersEngine;
        private NUnitCtr.Constraint internalConstraint;
        private bool isInitialized=false;
        
        public IComparer Comparer { get; set; }
        
        /// <summary>
        /// Request for metadata extraction
        /// </summary>
        public MembersDiscoveryRequest Request { get; protected set; }

        /// <summary>
        /// Engine dedicated to Members acquisition
        /// </summary>
        protected internal MembersAdomdEngine MembersEngine
        {
            get
            {
                if (membersEngine == null)
                    membersEngine = new MembersAdomdEngine();
                return membersEngine;
            }
            set
            {
                membersEngine = value ?? throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Construct a AbstractMembersConstraint
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
            if (!isInitialized)
                InitializeMatching();
                
            if (actual is MembersDiscoveryRequest)
                return Process((MembersDiscoveryRequest)actual);
            else if (actual is MemberResult)
            {
                this.actual = actual;
                var ctr = internalConstraint;
                if (ctr is NUnitCtr.CollectionItemsEqualConstraint)
                    ctr = ((NUnitCtr.CollectionItemsEqualConstraint)ctr).Using(Comparer);
                var res = DoMatch(ctr);
                return res;
            }
            else
                throw new ArgumentException();
        }

        protected virtual bool DoMatch(NUnitCtr.Constraint ctr)
        {
            return ctr.Matches(actual);
        }


        private void InitializeMatching()
        {
            PreInitializeMatching();
            internalConstraint = BuildInternalConstraint();
            if (internalConstraint!=null)
                isInitialized = true;
        }

        protected virtual void PreInitializeMatching()
        {
            
        }

        protected abstract NUnitCtr.Constraint BuildInternalConstraint();

        protected bool Process(MembersDiscoveryRequest actual)
        {
            Request = actual;
            var engine = MembersEngine;
            MemberResult result = engine.GetMembers(Request);
            return this.Matches(result);
        }

        #endregion

    }
}
