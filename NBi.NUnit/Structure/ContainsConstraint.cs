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
    public class ContainsConstraint : NUnitCtr.CollectionContainsConstraint
    {
        protected object _expected;
        protected string expectedCaption;
        protected string expectedDisplayFolder;
        protected IComparer comparer;
        protected MetadataDiscoveryRequest request;
        protected AdomdDiscoveryCommandFactory commandFactory;
        
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
            : base(StringComparerHelper.Build(expected))
        {
            _expected = expected;
            expectedCaption = expected;
            comparer = new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(true);
        }

        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainsConstraint(IFieldWithDisplayFolder expected)
            : base(expected)
        {
            _expected = expected;
            expectedCaption = expected.Caption;
            comparer = new NBi.Core.Analysis.Metadata.Field.ComparerByCaptionAndDisplayFolder(true);
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public new ContainsConstraint IgnoreCase
        {
            get
            {
                comparer = new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(false);
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
                base.Using(comparer);
                var res= base.Matches(actual);
                return res; 
            }
        }

        public bool doMatch(IEnumerable<IField> actual)
        {
           return base.Using(comparer).Matches(actual);
        }

        
        protected bool Process(MetadataDiscoveryRequest actual)
        {
            request = actual;
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
            if (request != null)
            {
                var description = new DescriptionStructureHelper();
                var filterExpression = description.GetFilterExpression(request.GetAllFilters());
                var nextTargetExpression = description.GetNextTargetExpression(request.Target);
                var expectationExpression = expectedCaption;

                writer.WritePredicate(string.Format("find a {0} named '{1}' contained {2}",
                    nextTargetExpression,
                    expectationExpression,
                    filterExpression));
                                                                               
            }
            else
                base.WriteDescriptionTo(writer);


        }

    }
}
