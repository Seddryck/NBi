using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Core.Structure;

namespace NBi.NUnit.Structure
{
    public abstract class AbstractStructureConstraint : NBiConstraint
    {
        //Internal Constraint is not necessary an CollectionItemsEqualConstraint
        //By expl, for ContainConstraint we've a collection of constrain and not just one constraint
        protected virtual NUnitCtr.Constraint InternalConstraint {get; set;}
        
        public IComparer Comparer { get; set; }
        
        /// <summary>
        /// Request for metadata extraction
        /// </summary>
        public IStructureDiscoveryCommand Command { get; protected set; }
        public IStructureDiscoveryCommand InvestigateCommand { get; protected set; }


        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public AbstractStructureConstraint()
        {
            Comparer = new Comparer(System.Threading.Thread.CurrentThread.CurrentCulture);
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        protected AbstractStructureConstraint IgnoreCase()
        {
            Comparer = new CaseInsensitiveComparer();
            return this;
        }

        /// <summary>
        /// Command to be executed when invesrtigating a failure
        /// </summary>
        protected AbstractStructureConstraint Investigate(StructureDiscoveryCommand command)
        {
            InvestigateCommand = command;
            return this;
        }

        #endregion
        
        #region Specific NUnit
        public override bool Matches(object actual)
        {
            if (actual is IStructureDiscoveryCommand)
                return Process((IStructureDiscoveryCommand)actual);
            else if (actual is IEnumerable<string>)
            {
                this.actual = actual;
                var ctr = InternalConstraint;
                if (InternalConstraint is CollectionItemsEqualConstraint)
                    //Only the type CollectionItemsEqualConstraint is supporting Using()
                    //Others constraint are mostly a composition of constraints and the comparer is applied to each constraint.
                    ctr = ((CollectionItemsEqualConstraint)ctr).Using(Comparer);
                var res = ctr.Matches(actual);
                return res;
            }
            else
                throw new ArgumentException();
        }

        protected bool Process(IStructureDiscoveryCommand actual)
        {
            Command = actual;
            IEnumerable<string> structures = Command.Execute().ToArray();
            return this.Matches(structures);
        }

        #endregion

    }
}
