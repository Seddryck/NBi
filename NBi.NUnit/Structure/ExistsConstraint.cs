using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core;
using NBi.Core.Analysis.Metadata;
using NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure
{
    public class ExistsConstraint : AbstractStructureConstraint
    {
        protected string Expected
        {
            get
            {
                return Request.GetAllFilters().Single(f => f.Target == Request.Target).Value;
            }
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
                base.IgnoreCase();
                return this;
            }
        }

        #endregion

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

        protected override Constraint InternalConstraint
        {
            get
            {
                if (base.InternalConstraint==null)
                    base.InternalConstraint = new CollectionContainsConstraint(StringComparerHelper.Build(Expected));
                return base.InternalConstraint;
            }
            set
            {
                base.InternalConstraint = value;
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

                writer.WritePredicate(string.Format("find {0} {1} named '{2}' {3}"
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

                var closeMatch = GetCloseMatch();
                if (!string.IsNullOrEmpty(closeMatch))
                {
                    writer.WriteMessageLine("");
                    writer.WriteMessageLine("");
                    writer.WriteMessageLine(string.Format("The value '{0}' is close to your expectation.", closeMatch));
                    writer.DisplayStringDifferences(Expected, closeMatch, -1, false, true);
                }
            }
        }

        protected virtual string GetCloseMatch()
        {
            var closestDistance = int.MaxValue;
            var closestValue = string.Empty;

            foreach (IField value in ((IEnumerable<IField>)actual))
            {
                var dist = value.Caption.LevenshteinDistance(Expected);
                if ( closestDistance > dist )
                {
                    closestDistance = dist;
                    closestValue = value.Caption;
                }
            }

            if (closestDistance <= 3)
                return closestValue;

            foreach (IField value in ((IEnumerable<IField>)actual))
            {
                var dist = value.Caption.RemoveDiacritics().LevenshteinDistance(Expected.RemoveDiacritics());
                if (closestDistance > dist)
                {
                    closestDistance = dist;
                    closestValue = value.Caption;
                }
            }

            if (closestDistance <= 3)
                return closestValue;

            return string.Empty;
        }

        
        
    }
}
