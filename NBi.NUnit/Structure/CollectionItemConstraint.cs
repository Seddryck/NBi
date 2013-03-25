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
    public class CollectionItemConstraint : NUnitCtr.CollectionContainsConstraint
    {
        public IComparer Comparer { get; set; }
        protected internal string Expected;
        protected AdomdDiscoveryCommandFactory commandFactory;       

        /// <summary>
        /// Request for metadata extraction
        /// </summary>
        public MetadataDiscoveryRequest Request { get; protected set; }
        
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
        public CollectionItemConstraint(string expected)
            : base(StringComparerHelper.Build(expected))
        {
            Expected = expected;
            Comparer = new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(true);
            base.Using(Comparer);
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public new CollectionItemConstraint IgnoreCase
        {
            get
            {
                Comparer = new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(false);
                return this;
            }
        }


        #endregion

        #region Specific NUnit
        public override bool Matches(object actual)
        {
            if (actual is MetadataDiscoveryRequest)
                return Process((MetadataDiscoveryRequest)actual);
            else
            {
                var res = base.Matches(actual);
                return res;
            }
        }

        public bool doMatch(IEnumerable<IField> actual)
        {
            return this.Matches(actual);
        }


        protected bool Process(MetadataDiscoveryRequest actual)
        {
            Request = actual;
            var factory = GetFactory();
            var command = factory.BuildExact(actual);
            IEnumerable<IField> structures = command.Execute();
            return this.Matches(structures);
        }
        #endregion

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            var description = new DescriptionStructureHelper();
            var filterExpression = description.GetFilterExpression(Request.GetAllFilters());
            var nextTargetExpression = description.GetNextTargetExpression(Request.Target);
            var expectationExpression = Expected;

            writer.WritePredicate(string.Format("find a {0} named '{1}' contained {2}",
                nextTargetExpression,
                expectationExpression,
                filterExpression));
        }

        public override void WriteActualValueTo(MessageWriter writer)
        {
            if (actual is IEnumerable<IField> && ((IEnumerable<IField>)actual).Count() > 0)
                base.WriteActualValueTo(writer);
            else
                writer.WriteActualValue(new WriterHelper.NothingFoundMessage());
        }
    }
}
