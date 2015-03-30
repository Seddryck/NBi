using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core;
using NBi.Core.Structure;
using NUnit.Framework.Constraints;

namespace NBi.NUnit.Structure
{
    public class ExistsConstraint : ContainConstraint
    {
        private new string Expected
        {
            get { return base.Expected.ElementAt(0); }
        }

        /// <summary>
        /// Construct a ExistsConstraint
        /// </summary>
        public ExistsConstraint(string expected)
            : base(expected)
        {
        }

        public new ExistsConstraint IgnoreCase
        {
            get
            {
                return (base.IgnoreCase() as ExistsConstraint);
            }
        }

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if (Command != null)
            {
                var description = new DescriptionStructureHelper();
                var filterExpression = description.GetFilterExpression(Command.Description.Filters.Where(f => f.Target != Command.Description.Target));
                var notExpression = description.GetNotExpression(true);
                var targetExpression = description.GetTargetExpression(Command.Description.Target);
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
            if (!(actual is IEnumerable<string>))
                return;

            var isApproximate = false;
            foreach (var actualItem in (actual as IEnumerable<string>))
            {
                var text = string.Empty;
                if (actualItem.ToLowerInvariant() == Expected.ToLowerInvariant())
                    text = string.Format("< <{0}> > (case not matching)", actualItem);
                else if (actualItem.TrimEnd() == Expected)
                    text = string.Format("< <{0}> > (with ending space(s))", actualItem);
                else if (actualItem.TrimStart() == Expected)
                    text = string.Format("< <{0}> > (with leading space(s))", actualItem);
                else if (actualItem.ToLowerInvariant().Trim() == Expected.ToLowerInvariant().Trim())
                    text = string.Format("< <{0}> > (small difference)", actualItem);

                if (!string.IsNullOrEmpty(text))
                {
                    writer.WriteActualValue(text);
                    isApproximate = true;
                }
            }

            if (!isApproximate)
            {
   
                if (((IEnumerable<string>)actual).Count() == 0)
                    writer.WriteActualValue(new WriterHelper.NothingFoundMessage());
                else
                {
                    base.WriteActualValueTo(writer);
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
        }

        protected virtual string GetCloseMatch()
        {
            var closestDistance = int.MaxValue;
            var closestValue = string.Empty;

            foreach (string value in ((IEnumerable<string>)actual))
            {
                var dist = value.LevenshteinDistance(Expected);
                if ( closestDistance > dist )
                {
                    closestDistance = dist;
                    closestValue = value;
                }
            }

            if (closestDistance <= 3)
                return closestValue;

            foreach (string value in ((IEnumerable<string>)actual))
            {
                var dist = value.RemoveDiacritics().LevenshteinDistance(Expected.RemoveDiacritics());
                if (closestDistance > dist)
                {
                    closestDistance = dist;
                    closestValue = value;
                }
            }

            if (closestDistance <= 3)
                return closestValue;

            return string.Empty;
        }
    }
}
