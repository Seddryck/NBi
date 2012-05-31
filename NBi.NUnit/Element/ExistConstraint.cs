using System;
using System.Collections;
using System.Collections.Generic;
using NBi.Core;
using NBi.Core.Analysis.Metadata;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Element
{
    public class ContainsConstraint : NUnitCtr.Constraint
    {
        protected string caption;
        protected IComparer comparer;
        protected MetadataAdomdExtractor _metadataExtractor;
        /// <summary>
        /// Engine dedicated to MetadataExtractor acquisition
        /// </summary>
        protected internal MetadataAdomdExtractor MetadataExtractor
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _metadataExtractor = value;
            }
        }

        protected MetadataAdomdExtractor GetEngine(string connectionString)
        {
            if (_metadataExtractor == null)
                _metadataExtractor = new MetadataAdomdExtractor(connectionString);
            return _metadataExtractor;
        }

        public ContainsConstraint()
        {
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

        public ContainsConstraint Caption(string value)
        {
            this.caption = value;
            return this;
        }

        #endregion

        public override bool Matches(object actual)
        {
            if (actual is IEnumerable<IElement>)
                return Matches((IEnumerable<IElement>)actual);
            else if (actual is MetadataQuery)
                return Matches((MetadataQuery)actual);

            return false;
        }

        /// <summary>
        /// Handle a string and check it with directly
        /// </summary>
        /// <param name="actual">a caption/unique key to find in the structure</param>
        /// <returns></returns>
        public bool Matches(IEnumerable<IElement> actual)
        {
            var ccc = new CollectionContainsConstraint(StringComparerHelper.Build(caption));
           var res = ccc.Using(comparer).Matches(actual);

           return res;
        }

        /// <summary>
        /// Handle a string and check it with directly
        /// </summary>
        /// <param name="actual">a caption/unique key to find in the structure</param>
        /// <returns></returns>
        public bool Matches(MetadataQuery actual)
        {
            var extr = GetEngine(actual.ConnectionString);
            IEnumerable<IElement> structures = extr.GetPartialMetadata(actual.Path, actual.Perspective);
            return Matches(structures);
        }



        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Structure not found");
            writer.WritePredicate(sb.ToString());
        }


    }
}
