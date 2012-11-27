using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Metadata;
using NBi.Core.Analysis.Metadata.Adomd;
using NBi.Core.Analysis.Request;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure
{
    public class ExistsConstraint : NUnitCtr.Constraint
    {

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
        public ExistsConstraint IgnoreCase
        {
            get
            {
                return this;
            }
        }

        #endregion

        public override bool Matches(object actual)
        {
            if (actual is MetadataDiscoveryRequest)
                return Process((MetadataDiscoveryRequest)actual);
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
                switch (request.Target)
                {
                    case DiscoveryTarget.Perspectives:
                        writer.WritePredicate(string.Format("On current cube, a perspective \"{0}\" exists"
                            , request.GetFilter(DiscoveryTarget.Perspectives).Value));
                        break;
                    case DiscoveryTarget.MeasureGroups:
                        writer.WritePredicate(string.Format("On perspective \"{0}\", the measuregroup \"{1}\" exists"
                            , request.GetFilter(DiscoveryTarget.Perspectives).Value
                            , request.GetFilter(DiscoveryTarget.MeasureGroups).Value));
                        break;
                    case DiscoveryTarget.Measures:
                        writer.WritePredicate(string.Format("On perspective \"{0}\", a measure \"{1}\" exists"
                            , request.GetFilter(DiscoveryTarget.Perspectives).Value
                            , request.GetFilter(DiscoveryTarget.Measures).Value));
                        break;
                    case DiscoveryTarget.Dimensions:
                        writer.WritePredicate(string.Format("On perspective \"{0}\", a dimension \"{1}\" exists"
                            , request.GetFilter(DiscoveryTarget.Perspectives).Value
                            , request.GetFilter(DiscoveryTarget.Dimensions).Value));
                        break;
                    default:
                        break;
                }
                
                writer.WriteExpectedValue(true);
            }
        }
    }
}
