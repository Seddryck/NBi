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
        protected NUnitCtr.CollectionItemsEqualConstraint realConstraint;
        protected object _expected;
        protected string expectedCaption;
        protected string expectedDisplayFolder;
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
        {
            realConstraint = new Contains.ContainsItemConstraint(expected);
            realConstraint = realConstraint.Using(new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(true));
        }

        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainsConstraint(IEnumerable<string> expected)
        {
            realConstraint = new Contains.ContainsEquivalentConstraint(expected);
            realConstraint = realConstraint.Using(new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(true));
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public ContainsConstraint IgnoreCase
        {
            get
            {
                realConstraint = realConstraint.Using(new NBi.Core.Analysis.Metadata.Field.ComparerByCaption(false));
                return this;
            }
        }

        //public ContainsConstraint Exactly
        //{
        //    get
        //    {
        //        //realConstraint = new Contains.ContainsEquivalentConstraint((IEnumerable)_expected);
        //        return this;
        //    }
        //}

        //public ContainsConstraint AtLeast
        //{
        //    get
        //    {
        //        if (_expected is string)
        //            realConstraint = new Contains.ContainsItemConstraint((string)_expected);
        //        else if (_expected is IEnumerable)
        //            realConstraint = new Contains.ContainsEquivalentConstraint((IEnumerable)_expected);
        //        else
        //            throw new ArgumentException();
                
        //        return this;
        //    }
        //}

        //public ContainsConstraint NotMore
        //{
        //    get
        //    {
        //        realConstraint = new Contains.ContainsEquivalentConstraint((IEnumerable)_expected);
        //        return this;
        //    }
        //}

        #endregion

        public override bool Matches(object actual)
        {
            if (actual is MetadataDiscoveryRequest)
                return Process((MetadataDiscoveryRequest)actual);
            else
            {
                var res = realConstraint.Matches(actual);
                return res; 
            }
        }

        public bool doMatch(IEnumerable<IField> actual)
        {
            return realConstraint.Matches(actual);
        }

        
        protected bool Process(MetadataDiscoveryRequest actual)
        {
            //realConstraint.Request = actual;
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
            realConstraint.WriteDescriptionTo(writer);
        }

    }
}
