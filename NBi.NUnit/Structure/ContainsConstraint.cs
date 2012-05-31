using System;
using System.Collections;
using System.Collections.Generic;
using NBi.Core;
using NBi.Core.Analysis.Metadata;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure
{
    public class ContainsConstraint : NUnitCtr.CollectionContainsConstraint
    {
        protected string expectedCaption;
        protected IComparer comparer;
        protected MetadataQuery metadataQuery;
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
            expectedCaption = expected;
            comparer = new NBi.Core.Analysis.Metadata.Element.ComparerByCaption(true);
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public ContainsConstraint IgnoreCase
        {
            get
            {
                comparer = new NBi.Core.Analysis.Metadata.Element.ComparerByCaption(false);
                return this;
            }
        }

        #endregion

        public override bool Matches(object actual)
        {
            if (actual is MetadataQuery)
                return Process((MetadataQuery)actual);
            else
            {
                base.Using(comparer);
                var res= base.Matches(actual);
                return res; 
            }
        }

        public bool doMatch(IEnumerable<IElement> actual)
        {
           return base.Using(comparer).Matches(actual);
        }

        
        protected bool Process(MetadataQuery actual)
        {
            metadataQuery = actual;
            var extr = GetEngine(actual.ConnectionString);
            IEnumerable<IElement> structures = extr.GetPartialMetadata(actual.Path, actual.Perspective);
            return this.Matches(structures);
        }

        /// <summary>
        /// Write a descripton of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if (metadataQuery != null)
            {
                var pathParser = PathParser.Build(metadataQuery.Path, metadataQuery.Perspective);
                writer.WritePredicate(string.Format("On perspective \"{0}\", a {1} identified by \"{2}\" containing a {3} with caption"
                                                            , metadataQuery.Perspective
                                                            , pathParser.Position.Current
                                                            , metadataQuery.Path
                                                            , pathParser.Position.Next));
                writer.WriteExpectedValue(expectedCaption);
            }
            else
                base.WriteDescriptionTo(writer);
            
        }
    }
}
