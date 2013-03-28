using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure
{
    public abstract class AbstractStructureConstraint : NUnitCtr.Constraint
    {
        private AdomdDiscoveryCommandFactory commandFactory;
        protected virtual NUnitCtr.CollectionItemsEqualConstraint InternalConstraint {get; set;}
        
        public IComparer Comparer { get; set; }
        
        /// <summary>
        /// Request for metadata extraction
        /// </summary>
        public MetadataDiscoveryRequest Request { get; protected set; }

        /// <summary>
        /// Engine dedicated to MetadataExtractor acquisition
        /// </summary>
        protected internal AdomdDiscoveryCommandFactory CommandFactory
        {
            get
            {
                if (commandFactory == null)
                    commandFactory = new AdomdDiscoveryCommandFactory();
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
        public AbstractStructureConstraint()
        {
            Comparer = new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(true);
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        protected void IgnoreCase()
        {
             Comparer = new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(false);
        }

        #endregion
        
        #region Specific NUnit
        public override bool Matches(object actual)
        {
            if (actual is MetadataDiscoveryRequest)
                return Process((MetadataDiscoveryRequest)actual);
            else if (actual is IEnumerable<IField>)
            {
                var ctr = InternalConstraint;
                ctr = ctr.Using(Comparer);
                var res = ctr.Matches(actual);
                return res;
            }
            else
                throw new ArgumentException();
        }

        protected bool Process(MetadataDiscoveryRequest actual)
        {
            Request = actual;
            var factory = CommandFactory;
            var command = BuildCommand(factory,actual);
            IEnumerable<IField> structures = command.Execute();
            this.actual = structures;
            return this.Matches(structures);
        }

        protected virtual AdomdDiscoveryCommand BuildCommand(AdomdDiscoveryCommandFactory factory, MetadataDiscoveryRequest actual)
        {
            return factory.BuildExact(actual);
        }
        #endregion

    }
}
