using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure
{
    public class ContainsConstraint : NUnitCtr.Constraint
    {
        protected internal NUnitCtr.CollectionItemsEqualConstraint RealConstraint;
        protected internal object Expected;
        protected AdomdDiscoveryCommandFactory commandFactory;
        protected IComparer comparer;
        
        
        /// <summary>
        /// Request for metadata extraction
        /// </summary>
        public MetadataDiscoveryRequest Request {get; protected set;}

        /// <summary>
        /// Engine dedicated to MetadataExtractor acquisition
        /// </summary>
        protected internal AdomdDiscoveryCommandFactory CommandFactory
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                commandFactory = value;
            }
        }

        protected AdomdDiscoveryCommandFactory GetFactory()
        {
            if (commandFactory == null)
                commandFactory = new AdomdDiscoveryCommandFactory();
            return commandFactory;
        }

        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainsConstraint(string expected)
        {
            this.Expected = expected;
            RealConstraint = new Contains.ContainsItemConstraint(expected, this);
            comparer = new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(true);
        }

        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainsConstraint(IEnumerable<string> expected)
        {
            this.Expected = expected;
            RealConstraint = new Contains.ContainsSubsetConstraint(expected, this);
            comparer = new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(true);
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public ContainsConstraint IgnoreCase
        {
            get
            {
                comparer = new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(false);
                return this;
            }
        }

        public ContainsConstraint Exactly
        {
            get
            {
                if (Expected is IEnumerable<string>)
                    RealConstraint = new Contains.ContainsEquivalentConstraint((IEnumerable<string>)Expected, this);
                else
                    throw new ArgumentException();

                return this;
            }
        }


        #endregion

        public override bool Matches(object actual)
        {
            if (actual is MetadataDiscoveryRequest)
                return Process((MetadataDiscoveryRequest)actual);
            else
            {
                RealConstraint = RealConstraint.Using(comparer);
                var res = RealConstraint.Matches(actual);
                return res; 
            }
        }

        public bool doMatch(IEnumerable<IField> actual)
        {
            return RealConstraint.Matches(actual);
        }

        
        protected bool Process(MetadataDiscoveryRequest actual)
        {
            Request = actual;
            var factory = GetFactory();
            var command = factory.BuildExact(actual);
            IEnumerable<IField> structures = command.Execute();
            return this.Matches(structures);
        }

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            RealConstraint.WriteDescriptionTo(writer);
        }

        /// <summary>
        /// Write the actual values of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteActualValueTo(MessageWriter writer)
        {
            RealConstraint.WriteActualValueTo(writer);
        }

    }
}
