using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBi.Core;
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
            InternalConstraint = new CollectionContainsConstraint(expected);
            this.Expected = expected;
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public new LinkedToConstraint IgnoreCase
        {
            get
            {
                base.IgnoreCase();
                return this;
            }
        }

        #endregion

        /// <summary>
        /// Write a description of the constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if (Command != null)
            {
                var description = new DescriptionStructureHelper();
                var notExpression = description.GetNotExpression(true);
                var targetExpression = description.GetTargetExpression(Command.Description.Target);
                var captionExpression = Expected;
                var filterExpression = description.GetFilterExpression(Command.Description.Filters.Where(f => f.Target != Command.Description.Target)).Remove(0,3);

                writer.WritePredicate(string.Format("find {0} {1} named '{2}' linked to {3}"
                            , notExpression
                            , targetExpression
                            , captionExpression
                            , filterExpression));
            }
        }

        public override void WriteActualValueTo(MessageWriter writer)
        {
            //actual is equal to the List of Dimensions/Measure-Group effectively linkedTo, so we don't need to perform an investigation.
            
            ////IF actual is not empty it means we've an issue with Casing or a space at the end
            //if (actual is IEnumerable<IField> && ((IEnumerable<IField>)actual).Count() == 1)
            //{
            //    if (((IEnumerable<IField>)actual).ToArray()[0].Caption.ToLowerInvariant() == Expected.ToLowerInvariant())
            //        writer.WriteActualValue(string.Format("< <{0}> > (case not matching)", ((IEnumerable<IField>)actual).ToArray()[0].Caption));
            //    else if (((IEnumerable<IField>)actual).ToArray()[0].Caption.EndsWith(" "))
            //        writer.WriteActualValue(string.Format("< <{0}> > (with ending space(s))", ((IEnumerable<IField>)actual).ToArray()[0].Caption));
            //    else
            //        writer.WriteActualValue(string.Format("< <{0}> > (small difference)", ((IEnumerable<IField>)actual).ToArray()[0].Caption));

            //}
            //else
            //{
            //    Investigate();
            if (actual is IEnumerable<string> && ((IEnumerable<string>)actual).Count() > 0)
                    base.WriteActualValueTo(writer);
                else
                    writer.WriteActualValue(new WriterHelper.NothingFoundMessage());
            //}
        }

        
        
    }
}
