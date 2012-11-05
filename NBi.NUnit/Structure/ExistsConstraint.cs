﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Discovery;
using NBi.Core.Analysis.Metadata;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Core.Analysis.Metadata.Adomd;

namespace NBi.NUnit.Structure
{
    public class ExistsConstraint : NUnitCtr.Constraint
    {

        protected IComparer comparer;
        protected MetadataDiscoveryCommand command;
        protected IMetadataExtractor metadataExtractor;

        /// <summary>
        /// Engine dedicated to MetadataExtractor acquisition
        /// </summary>
        protected internal IMetadataExtractor MetadataExtractor
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                metadataExtractor = value;
            }
        }

        protected IMetadataExtractor GetEngine(string connectionString)
        {
            if (metadataExtractor == null)
                metadataExtractor = new MetadataAdomdExtractor(connectionString);
            return metadataExtractor;
        }

        /// <summary>
        /// Construct a ExistsConstraint
        /// </summary>
        public ExistsConstraint()
            : base()
        {
            
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public new ExistsConstraint IgnoreCase
        {
            get
            {
                return this;
            }
        }

        #endregion

        public override bool Matches(object actual)
        {
            if (actual is MetadataDiscoveryCommand)
                return Process((MetadataDiscoveryCommand)actual);
            else if (actual is IEnumerable<IField>)
            {
                var res = doMatch((IEnumerable<IField>)actual);
                return res;
            }
            else
                throw new ArgumentException();
        }

        public bool doMatch(IEnumerable<IField> actual)
        {
           return (actual.Count()>0);
        }

        
        protected bool Process(MetadataDiscoveryCommand actual)
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
                if (command.Target == DiscoveryTarget.Perspectives)
                {
                    writer.WritePredicate(string.Format("On current cube, a perspective \"{1}\" exists"
                        , command.GetFilter(DiscoveryTarget.Perspectives).Value
                        ));
                }
                if (command.Target == DiscoveryTarget.MeasureGroups)
                {
                    writer.WritePredicate(string.Format("On perspective \"{0}\", the measuregroup \"{1}\" exists"
                        , command.GetFilter(DiscoveryTarget.Perspectives).Value
                        , command.GetFilter(DiscoveryTarget.MeasureGroups).Value
                        ));
                    if (command.Target == DiscoveryTarget.Measures)
                    {
                        writer.WritePredicate(string.Format("On perspective \"{0}\", a measure \"{1}\" exists"
                            , command.GetFilter(DiscoveryTarget.Perspectives).Value
                            , command.GetFilter(DiscoveryTarget.Measures).Value
                            ));
                    }
                    if (command.Target == DiscoveryTarget.Dimensions || command.Target == DiscoveryTarget.Hierarchies || command.Target == DiscoveryTarget.Levels)
                        writer.WritePredicate(string.Format("On perspective \"{0}\", a {1} identified by \"{2}\" containing a {3} with caption"
                            , command.GetFilter(DiscoveryTarget.Perspectives)
                            , command.GetDepthName()
                            , "TODO"
                            , command.GetNextDepthName()));

                    writer.WriteExpectedValue(true);
                }
                //else
                //    base.WriteDescriptionTo(writer);

            }
        }
    }
}
