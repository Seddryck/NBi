using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Core;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Core.Structure;

namespace NBi.NUnit.Structure
{
    public class ContainConstraint : AbstractCollectionConstraint
    {
        /// <summary>
        /// Construct a SomeItemsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainConstraint(string expected)
            : this(new List<string>() {expected})
        {
        }
        /// <summary>
        /// Construct a SomeItemsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public ContainConstraint(IEnumerable<string> expected)
            : base(expected)
        {
            BuildInternalConstraint();
        }

        /// <summary>
        /// Build a collection of constraint (in fact one by expected item)
        /// Pay really attention that SomeItemsConstraint is expecting one unique item!
        /// So if you cant to check for several items, you need to apply several Constraint (one by expected item)
        /// </summary>
        protected virtual void BuildInternalConstraint()
        {
            NUnitCtr.Constraint ctr = null;
            foreach (var item in Expected)
            {
                var usingCtr = new SomeItemsConstraint(new EqualConstraint(item).Using(Comparer));

                if (ctr != null)
                    ctr = new AndConstraint(ctr, usingCtr);
                else
                    ctr = usingCtr;
            }

            if (ctr == null)
                ctr = new NUnitCtr.EmptyCollectionConstraint();

            InternalConstraint = ctr;
        }

        #region Modifiers
        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// Pay attention that we need to rebuild the constraint from scratch
        /// because the Using is applied for each constraint contained in this constraint
        /// </summary>
        /// <value>The constraint configured to compare metadata captions without regard to case.</value>
        public new ContainConstraint IgnoreCase
        {
            get
            {
                base.IgnoreCase();
                BuildInternalConstraint();
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
            var description = new DescriptionStructureHelper();
            var filterExpression = description.GetFilterExpression(Command.Description.Filters.Where(f => f is CaptionFilter).Cast<CaptionFilter>());
            if (!string.IsNullOrEmpty(filterExpression))
                filterExpression = string.Format(" contained {0}", filterExpression);

            if (Expected.Count() == 1)
            {
                writer.Write(string.Format("find a {0} named '{1}'{2}.",
                    description.GetTargetExpression(Command.Description.Target),
                    Expected.First(),
                    filterExpression));
            }
            else
            {
                var expectationExpression = new StringBuilder();
                foreach (string item in Expected)
                    expectationExpression.AppendFormat("<{0}>, ", item);
                expectationExpression.Remove(expectationExpression.Length - 2, 2);

                writer.Write(string.Format("find the {0} named '{1}'{2}.",
                    description.GetTargetPluralExpression(Command.Description.Target),
                    expectationExpression.ToString(),
                    filterExpression));
            }
        }
    }
}
