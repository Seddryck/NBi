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
    public class LinkedToConstraint : AbstractStructureConstraint
    {
        protected string Expected {get; set;}

        /// <summary>
        /// Construct a LinkedToConstraint
        /// </summary>
        public LinkedToConstraint(string expected)
            : base()
        {
            InternalConstraint = new CollectionContainsConstraint(StringComparerHelper.Build(expected));
            this.Expected = expected;
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public LinkedToConstraint IgnoreCase
        {
            get
            {
                base.IgnoreCase();
                return this;
            }
        }

        #endregion

        /// <summary>
        /// Change the standard Build using BuildExact by BuildLinkedTo
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        protected override AdomdDiscoveryCommand BuildCommand(AdomdDiscoveryCommandFactory factory, MetadataDiscoveryRequest actual)
        {
            return factory.BuildLinkedTo(actual);
        }
        
        protected void Investigate()
        {
            var factory = CommandFactory;
            var command = factory.BuildExternal(Request);
            IEnumerable<IField> structures = command.Execute();

            if (structures.Count() > 0)
            {
                this.actual = structures;
            }
        }

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if (Request != null)
            {
                var description = new DescriptionStructureHelper();
                var filterExpression = description.GetFilterExpression(Request.GetAllFilters().Where(f => f.Target != Request.Target));
                var notExpression = description.GetNotExpression(true);
                var targetExpression = description.GetTargetExpression(Request.Target);
                var captionExpression = Expected;

                writer.WritePredicate(string.Format("find {0} {1} named '{2}' linked to {3}"
                            , notExpression
                            , targetExpression
                            , captionExpression
                            , filterExpression));
            }
        }

        public override void WriteActualValueTo(MessageWriter writer)
        {
            //IF actual is not empty it means we've an issue with Casing or a space at the end
            if (actual is IEnumerable<IField> && ((IEnumerable<IField>)actual).Count() == 1)
            {
                if (((IEnumerable<IField>)actual).ToArray()[0].Caption.ToLowerInvariant() == Expected.ToLowerInvariant())
                    writer.WriteActualValue(string.Format("< <{0}> > (case not matching)", ((IEnumerable<IField>)actual).ToArray()[0].Caption));
                else if (((IEnumerable<IField>)actual).ToArray()[0].Caption.EndsWith(" "))
                    writer.WriteActualValue(string.Format("< <{0}> > (with ending space(s))", ((IEnumerable<IField>)actual).ToArray()[0].Caption));
                else
                    writer.WriteActualValue(string.Format("< <{0}> > (small difference)", ((IEnumerable<IField>)actual).ToArray()[0].Caption));

            }
            else
            {
                Investigate();
                if (actual is IEnumerable<IField> && ((IEnumerable<IField>)actual).Count() > 0)
                    base.WriteActualValueTo(writer);
                else
                    writer.WriteActualValue(new WriterHelper.NothingFoundMessage());
            }
        }

        
        
    }
}
