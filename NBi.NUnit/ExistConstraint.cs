using System;
using System.Collections.Generic;
using NBi.Core.Analysis.Metadata;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class ExistConstraint : NUnitCtr.Constraint
    {
        protected string caption;
        protected Structure.Comparer comparer;
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

        public ExistConstraint()
        {

        }

        public ExistConstraint Caption(string value)
        {
            this.caption = value;
            this.comparer = new Structure.ComparerByCaption(false);
            return this;
        }

        public override bool Matches(object actual)
        {
            if (actual is IEnumerable<IStructure>)
                return Matches((IEnumerable<IStructure>)actual);
            else if (actual is MetadataQuery)
                return Matches((MetadataQuery)actual);

            return false;
        }

        /// <summary>
        /// Handle a string and check it with directly
        /// </summary>
        /// <param name="actual">a caption/unique key to find in the structure</param>
        /// <returns></returns>
        public bool Matches(IEnumerable<IStructure> actual)
        {
           var ccc = new CollectionContainsConstraint(new Structure(caption));
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
            IEnumerable<IStructure> structures = extr.GetChildStructure(actual.Path, actual.Perspective);
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
