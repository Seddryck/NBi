using System;
using System.Collections;
using System.Collections.Generic;
using NBi.Core;
using NBi.Core.Analysis;
using NBi.Core.Analysis.Metadata;
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
        protected DiscoverCommand command;
        protected IMetadataExtractor _metadataExtractor;
        
        /// <summary>
        /// Engine dedicated to MetadataExtractor acquisition
        /// </summary>
        protected internal IMetadataExtractor MetadataExtractor
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _metadataExtractor = value;
            }
        }

        protected IMetadataExtractor GetEngine(string connectionString)
        {
            if (_metadataExtractor == null)
                _metadataExtractor = new MetadataAdomdExtractor(connectionString);
            return _metadataExtractor;
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
            if (actual is DiscoverCommand)
                return Process((DiscoverCommand)actual);
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

        
        protected bool Process(DiscoverCommand actual)
        {
            command = actual;
            var extr = GetEngine(actual.ConnectionString);
            IEnumerable<IField> structures = extr.GetPartialMetadata(actual);
            return this.Matches(structures);
        }

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if (command != null)
            {
                var pathParser = PathParser.Build(command);

                if (command.Target == DiscoverTarget.Perspectives)
                {
                    writer.WritePredicate(string.Format("On current cube, a perspective with caption"));
                }
                else if (command.Target == DiscoverTarget.Measures || command.Target == DiscoverTarget.MeasureGroups)
                {
                    var displayFolder = (_expected is IFieldWithDisplayFolder) ? string.Format(", in folder \"{0}\", ", ((IFieldWithDisplayFolder)_expected).DisplayFolder) : string.Empty;
                    writer.WritePredicate(string.Format("On perspective \"{0}\", the measuregroup \"{1}\" containing{2}a measure with caption"
                                                                               , pathParser.Filter.Perspective
                                                                               , pathParser.Filter.MeasureGroupName
                                                                               , displayFolder));
                }
                else
                    writer.WritePredicate(string.Format("On perspective \"{0}\", a {1} identified by \"{2}\" containing a {3} with caption"
                                                            , pathParser.Filter.Perspective
                                                            , pathParser.Position.Current
                                                            , command.Path
                                                            , pathParser.Position.Next));
                writer.WriteExpectedValue(expectedCaption);
            }
            else
                base.WriteDescriptionTo(writer);
            
        }
    }
}
